// 日本語対応
using UnityEngine;
using HitSupport;
using UniRx;
using System;

public class PlayerInfo : MonoBehaviour, IItemUser
{
    private static PlayerInfo _currentPlayerInfo = default;
    public static PlayerInfo CurrentPlayerInfo => _currentPlayerInfo;
    public Transform ItemUserTransform => transform;

    [SerializeField]
    private SphereCast _grounded = default;
    [SerializeField]
    private float _moveSpeed = 8f;
    [SerializeField]
    private float _moveDeceleration = 12f;
    [SerializeField]
    private float _jumpPower = 8f;

    private ReactiveProperty<PlayerState> _currentState = new ReactiveProperty<PlayerState>(0);
    private InGameInput _inputs = null;
    private BoolReactiveProperty _isGrounded = new BoolReactiveProperty(false);

    public InGameInput Inputs => _inputs;
    public IReadOnlyReactiveProperty<PlayerState> CurrentState => _currentState;

    public void AddState(PlayerState state)
    {
        _currentState.Value |= state;
    }
    public void RemoveState(PlayerState state)
    {
        _currentState.Value &= ~state;
    }

    private void Awake()
    {
        _inputs = new InGameInput();
        _inputs.Enable();
        _currentPlayerInfo = this;
    }
    private void OnDestroy()
    {
        _currentPlayerInfo = null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        _grounded.OnDrawGizmos();
    }
#endif

    public IReadOnlyReactiveProperty<bool> IsGrounded
    {
        get
        {
            _isGrounded.Value = _grounded.IsHit(transform);
            return _isGrounded;
        }
    }
    public float GroundMoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
    }
    public float GroundDeceleration
    {
        get
        {
            return _moveDeceleration;
        }
    }
    public float JumpPower
    {
        get
        {
            return _jumpPower;
        }
    }
    public float AttackPower
    {
        get
        {
            return 10f;
        }
    }
}
[Serializable]
[Flags]
public enum PlayerState : int
{
    Run = 1,
    Attack = 2,
    Midair = 4,
    Land = 8,
    Talk = 16
}
