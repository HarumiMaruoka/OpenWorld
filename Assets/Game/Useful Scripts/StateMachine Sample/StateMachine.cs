using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    [Tooltip("�ŏ��̃X�e�[�g"), SerializeField]
    private State<T> _initilizeState = default;
    /// <summary> ���݂̃X�e�[�g��\������l </summary>
    public State<T> CurrentState { get; private set; } = null;
    /// <summary> ���̒l��ύX���鎖�ŏ�Ԃ�J�ڂ���B </summary>
    public T Conditions { get; protected set; } = default;
    /// <summary> �X�e�[�g���i�[���Ă����R���e�i </summary>
    private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();
    /// <summary> �X�e�[�g���i�[���Ă����R���e�i </summary>
    public IReadOnlyDictionary<Type, State<T>> States => _states;

    private void Start()
    {
        // �ŏ��̃X�e�[�g�����蓖�Ă�
        var state = ChangeState(_initilizeState);
        state.ExecuteEnter();
    }

    public State<T> ChangeState(State<T> nextState)
    {
        // �f�B�N�V���i���ɓo�^����Ă��鎞�͓o�^����Ă���I�u�W�F�N�g�����̂܂ܗ��p����
        if (_states.TryGetValue(nextState.GetType(), out State<T> value))
        {
            return CurrentState = value;
        }
        // �o�^����Ă��Ȃ��ꍇ
        else
        {
            // �X�N���v�^�u���I�u�W�F�N�g�̕������쐬����
            var clone = (State<T>)ScriptableObject.CreateInstance(nextState.GetType());
            // �N���[���̃Z�b�g�A�b�v���������s
            clone.Setup(this, nextState);
            clone.ThisAwake();
            // �f�B�N�V���i���ɓo�^����
            _states.Add(clone.GetType(), clone);
            // ���݃X�e�[�g���X�V����
            return CurrentState = clone;
        }
    }
    private void Update()
    {
        CurrentState.Execute();
    }
    public abstract void ClearCondition();
    public abstract void AddCondition(T condition);
    public abstract void RemoveCondition(T condition);
}
/// <summary>
/// �J�ڂ̏�����\������񋓑�
/// </summary>
//[Flags, Serializable]
//public enum Conditions
//{
//    None = 0,
//    Everything = -1,
//    Idle = 1,
//    Move = 2,
//    Jump = 4,
//}