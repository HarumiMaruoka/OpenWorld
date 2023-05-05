// 日本語対応
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class PlayerAnimationController : MonoBehaviour
{
    [AnimationParameter, SerializeField]
    private string _moveSpeedParam = default;
    [AnimationParameter, SerializeField]
    private string _attackParam = default;
    [AnimationParameter, SerializeField]
    private string _jumpParam = default;
    [AnimationParameter, SerializeField]
    private string _midairParam = default;

    private Animator _animator = null;
    private PlayerInfo _info = null;
    private PlayerMove _move = null;
    private PlayerAttack _attack = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _info = GetComponent<PlayerInfo>();
        _move = GetComponent<PlayerMove>();
        _attack = GetComponent<PlayerAttack>();

        _move.CurrentHorizontalSpeed.Subscribe(value =>
            _animator.SetFloat(_moveSpeedParam, value / _move.MaxHorizontalSpeed));

        _attack.IsAttacking.Subscribe(value =>
            _animator.SetBool(_attackParam, value));

        _info.IsGrounded.Subscribe(async value =>
            {
                _animator.SetBool(_midairParam, !value);
                if (!value)
                {
                    _animator.SetBool(_jumpParam, true);
                    await UniTask.DelayFrame(1);
                    _animator.SetBool(_jumpParam, false);
                }
            });
    }
}
