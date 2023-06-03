// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemy_State_Idle", menuName = "ScriptableObjects/SampleEnemyState/Idle", order = 1)]
public class SampleEnemy_State_Idle : EnemyStateBase
{
    protected override void Enter()
    {
        //Debug.Log("アイドル開始");
    }

    protected override void Exit()
    {
        //Debug.Log("アイドル終了");
    }

    protected override void Update()
    {
        //Debug.Log("アイドル中");
    }
}
