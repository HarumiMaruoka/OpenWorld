// 日本語対応
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class PlayerAnimationController : MonoBehaviour
{
    [AnimationParameter, SerializeField]
    private string _moveSpeedParam = default;
    [AnimationParameter, SerializeField]
    private string _attackParam = default;
    [AnimationParameter, SerializeField]
    private string _midairParam = default;
    [AnimationParameter, SerializeField]
    private string _avoidanceParam = default;

    private Animator _animator = null;
    private PlayerInfo _info = null;
    private PlayerMove _move = null;
    private PlayerAttack _attack = null;
    private PlayerAvoidance _avoidance = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _info = GetComponent<PlayerInfo>();
        _move = GetComponent<PlayerMove>();
        _attack = GetComponent<PlayerAttack>();
        _avoidance = GetComponent<PlayerAvoidance>();

        _move.CurrentHorizontalSpeed.Subscribe(value =>
            _animator.SetFloat(_moveSpeedParam, value / _move.MaxHorizontalSpeed));

        _attack.IsAttacking.Subscribe(value =>
            _animator.SetBool(_attackParam, value));

        _info.IsGrounded.Subscribe(value =>
            _animator.SetBool(_midairParam, !value));

        _avoidance.IsAvoiding.Subscribe(value =>
            _animator.SetBool(_avoidanceParam, value));
    }

    // === 下記は アニメーションをインスペクタ(UnityEvent)を利用して 制御する用のメソッド === //
    public void SetAnimationParam(string animParamName)
    {
        _animator.SetBool(animParamName, true);
    }
    public void UnsetAnimationParam(string animParamName)
    {
        _animator.SetBool(animParamName, false);
    }
    public async void SetAnimationParamOneFrame(string animParamName)
    {
        _animator.SetBool(animParamName, true);
        await UniTask.Yield();
        _animator.SetBool(animParamName, false);
    }
}
