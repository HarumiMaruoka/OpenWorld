// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(fileName = "SampleEnemy_State_Search", menuName = "ScriptableObjects/SampleEnemyState/Search", order = 3)]
public class SampleEnemy_State_Search : EnemyStateBase
{
    private SampleEnemySearch _sampleEnemySearch = null;
    private EnemySight _enemySight = null;

    private SampleEnemyController _sampleEnemyController = null;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private bool _isFindPlayer = false;

    public override void ThisAwake()
    {
        _sampleEnemySearch = _owner.GetComponent<SampleEnemySearch>();
        _enemySight = _owner.GetComponent<EnemySight>();
        _sampleEnemyController = _owner.GetComponent<SampleEnemyController>();
        _sampleEnemySearch.OnSearchCompleted += OnSearchEnd;
    }
    protected override void Enter()
    {
        //Debug.Log("プレイヤー探し開始");

        Reaction();

        _isFindPlayer = false;

        var token = _cancellationTokenSource.Token;
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_owner.GetCancellationTokenOnDestroy(), token).Token;

#pragma warning disable 4014
        _sampleEnemySearch.Search(linkedToken);
    }

    protected override void Exit()
    {
        //Debug.Log("プレイヤー探し終了");
        _sampleEnemySearch.Kill();
    }

    protected override void Update()
    {
        //Debug.Log("プレイヤー探し中");
        if (_enemySight.PlayerIsVisible()) // プレイヤーが見つかったとき
        {
            _cancellationTokenSource.Cancel();
            _owner.RemoveCondition(EnemyConditionList.Search);
            OnSearchEnd();
            _isFindPlayer = true;
        }
    }

    private void OnSearchEnd() // プレイヤーが見つからなかったとき
    {
        _owner.RemoveCondition(EnemyConditionList.Tracking);
        if (_isFindPlayer)
        {
            _owner.RemoveCondition(EnemyConditionList.Search);
            _owner.AddCondition(EnemyConditionList.Tracking);
        }
        else
        {
            _owner.RemoveCondition(EnemyConditionList.Search);
            _owner.AddCondition(EnemyConditionList.RandomWalk);
        }
    }
    [SerializeField]
    private float _reactionTime = 1f;
    private async void Reaction()
    {
        //「?マーク」を表示する。（!マークが表示されていたら!マークは閉じる）
        if (_sampleEnemyController.ExclamationMark.IsOpened)
            _sampleEnemyController.ExclamationMark.Close();
        _sampleEnemyController.QuestionMark.Open();
        await UniTask.Delay((int)(_reactionTime * 1000f));
        _sampleEnemyController.QuestionMark.Close();
    }
}
