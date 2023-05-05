// 日本語対応
using UnityEngine;
using HitSupport;
using UniRx;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private OverLapSphere _grounded = default;
    [SerializeField]
    private float _moveSpeed = 8f;
    [SerializeField]
    private float _moveDeceleration = 12f;

    private InGameInput _inputs = null;
    private BoolReactiveProperty _isGrounded = new BoolReactiveProperty(false);

    public InGameInput Inputs => _inputs;

    public bool CanMove { get; set; } = true;

    private void Awake()
    {
        _inputs = new InGameInput();
        _inputs.Enable();
    }
    private void OnDrawGizmosSelected()
    {
        _grounded.OnDrawGizmos(transform);
    }


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
            return 8f;
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
