// 日本語対応
using System;

public class EnemyStateMachine : StateMachine<EnemyConditionList>
{
    public override void ClearCondition()
    {
        Conditions = 0;
    }
    public override void AddCondition(EnemyConditionList condition)
    {
        Conditions |= condition;
    }
    public override void RemoveCondition(EnemyConditionList condition)
    {
        Conditions &= ~condition;
    }
}
[Flags, Serializable]
public enum EnemyConditionList
{
    None = 0,
    Everything = -1,
    Idle = 1,
    Move = 2,
    Attack = 4,
}