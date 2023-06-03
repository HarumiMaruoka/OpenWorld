// 日本語対応
using UnityEngine;

public class SampleEnemyController : MonoBehaviour
{
    [SerializeField]
    private float _idleTime = 3f;
    [SerializeField]
    private float _moveTime = 3f;
    [SerializeField]
    private float _attackTime = 3f;

    private EnemyStateMachine _enemyStateMachine = default;
    private float _timer = 0f;

    private void Start()
    {
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        // Idle→Move→Attack→Idle を繰り返す
        _timer += Time.deltaTime;
        if (_enemyStateMachine.CurrentState.GetType() == typeof(SampleEnemy_State_Idle))
        {
            if (_timer > _idleTime)
            {
                _timer -= _idleTime;
                _enemyStateMachine.AddCondition(EnemyConditionList.Move);
                _enemyStateMachine.RemoveCondition(EnemyConditionList.Idle);
            }
        }
        else if (_enemyStateMachine.CurrentState.GetType() == typeof(SampleEnemy_State_Move))
        {
            if (_timer > _moveTime)
            {
                _timer -= _moveTime;
                _enemyStateMachine.AddCondition(EnemyConditionList.Attack);
                _enemyStateMachine.RemoveCondition(EnemyConditionList.Move);
            }
        }
        else if (_enemyStateMachine.CurrentState.GetType() == typeof(SampleEnemy_State_Attack))
        {
            if (_timer > _attackTime)
            {
                _timer -= _attackTime;
                _enemyStateMachine.AddCondition(EnemyConditionList.Idle);
                _enemyStateMachine.RemoveCondition(EnemyConditionList.Attack);
            }
        }
    }
}
