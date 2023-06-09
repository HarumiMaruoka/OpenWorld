// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemy_State_RandomWalk", menuName = "ScriptableObjects/SampleEnemyState/RandomWalk", order = 1)]
public class SampleEnemy_State_RandomWalk : EnemyStateBase
{
    private Actor_RandomWalk _randomWalk = null;
    private EnemySight _enemySight = null;
    public override void ThisAwake()
    {
        _randomWalk = _owner.GetComponent<Actor_RandomWalk>();
        _enemySight = _owner.GetComponent<EnemySight>();
    }
    protected override void Enter()
    {
        //Debug.Log("ランダムウォーク開始");
        _randomWalk.enabled = true;
    }

    protected override void Exit()
    {
        //Debug.Log("ランダムウォーク終了");
        _randomWalk.enabled = false;
    }

    protected override void Update()
    {
        //Debug.Log("ランダムウォーク中");
        if (_enemySight.PlayerIsVisible()) // プレイヤーを見つけたとき
        {
            _owner.RemoveCondition(EnemyConditionList.RandomWalk);
            _owner.AddCondition(EnemyConditionList.Tracking);
        }
    }
}
