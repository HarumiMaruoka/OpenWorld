// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemy_State_Tracking", menuName = "ScriptableObjects/SampleEnemyState/Tracking", order = 2)]
public class SampleEnemy_State_Tracking : EnemyStateBase
{
    [SerializeField]
    private float _attackRange = 5f;

    private SampleEnemyController _sampleEnemyController = null;
    private SampleEnemyTracking _tracking = null;
    private EnemySight _enemySight = null;

    public override void ThisAwake()
    {
        _tracking = _owner.GetComponent<SampleEnemyTracking>();
        _enemySight = _owner.GetComponent<EnemySight>();
        _sampleEnemyController = _owner.GetComponent<SampleEnemyController>();
    }
    protected override void Enter()
    {
        //Debug.Log("追跡開始");

        Reaction();

        _tracking.enabled = true;
    }

    protected override void Exit()
    {
        //Debug.Log("追跡終了");
        _tracking.enabled = false;
        _playerIsNotFound = false;
    }

    private float _timer = 0f;
    private float _time = 1f;
    private bool _playerIsNotFound = false;

    protected override void Update()
    {
        //Debug.Log("追跡中");
        if (!_enemySight.PlayerIsVisible())
        {
            _timer += Time.deltaTime;
            if (_time < _timer)
            {
                _playerIsNotFound = true;
            }
        }
        else
        {
            _timer = 0f;
        }

        if (_playerIsNotFound) // 継続して1秒プレイヤーが視界に入らなかったとき
        {
            _owner.RemoveCondition(EnemyConditionList.Tracking);
            _owner.AddCondition(EnemyConditionList.Search);
        }
        if (PlayerInfo.CurrentPlayerInfo != null &&
            Vector3.SqrMagnitude(PlayerInfo.CurrentPlayerInfo.transform.position - _owner.transform.position) < _attackRange)
        {
            _owner.RemoveCondition(EnemyConditionList.Tracking);
            _owner.AddCondition(EnemyConditionList.Attack);
        }
    }
    [SerializeField]
    private float _reactionTime = 1f;
    private async void Reaction()
    {
        // !マーク を表示する。（?マークが表示されていたら?マークは閉じる）
        if (_sampleEnemyController.QuestionMark.IsOpened)
            _sampleEnemyController.QuestionMark.Close();
        _sampleEnemyController.ExclamationMark.Open();
        await UniTask.Delay((int)(_reactionTime * 1000f));
        _sampleEnemyController.ExclamationMark.Close();
    }
}
