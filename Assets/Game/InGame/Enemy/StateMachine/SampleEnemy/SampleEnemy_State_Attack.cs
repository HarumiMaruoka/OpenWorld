// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemy_State_Attack", menuName = "ScriptableObjects/SampleEnemyState/Attack", order = 3)]
public class SampleEnemy_State_Attack : EnemyStateBase
{
    protected override void Enter()
    {
        //Debug.Log("攻撃開始");
    }

    protected override void Exit()
    {
        //Debug.Log("攻撃終了");
    }

    protected override void Update()
    {
        //Debug.Log("攻撃中");
    }
}
