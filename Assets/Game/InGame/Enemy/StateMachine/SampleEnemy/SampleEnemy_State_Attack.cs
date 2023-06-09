// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "SampleEnemy_State_Attack", menuName = "ScriptableObjects/SampleEnemyState/Attack", order = 3)]
public class SampleEnemy_State_Attack : EnemyStateBase
{
    private SampleEnemyAttack _attack = null;

    public override void ThisAwake()
    {
        _attack = _owner.GetComponent<SampleEnemyAttack>();
    }

    protected override void Enter()
    {
        //Debug.Log("攻撃開始");
        _attack.Fire(_owner.GetCancellationTokenOnDestroy());
        _attack.OnAttackCompleted += OnAttackEnd;
    }

    protected override void Exit()
    {
        //Debug.Log("攻撃終了");
    }

    protected override void Update()
    {
        //Debug.Log("攻撃中");
    }

    private void OnAttackEnd()
    {
        _owner.RemoveCondition(EnemyConditionList.Attack);
        _owner.AddCondition(EnemyConditionList.Search);
    }
}
