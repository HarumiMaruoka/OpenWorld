// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemy_State_Move", menuName = "ScriptableObjects/SampleEnemyState/Move", order = 2)]
public class SampleEnemy_State_Move : EnemyStateBase
{
    protected override void Enter()
    {
        //Debug.Log("移動開始");
    }

    protected override void Exit()
    {
        //Debug.Log("移動終了");
    }

    protected override void Update()
    {
        //Debug.Log("移動中");
    }
}
