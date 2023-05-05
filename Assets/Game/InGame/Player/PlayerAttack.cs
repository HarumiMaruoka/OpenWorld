// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using HitSupport;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private OverLapBox _attackArea = default;

    private PlayerInfo _info = null;
    private BoolReactiveProperty _isAttacking = new BoolReactiveProperty(false);

    public IReadOnlyReactiveProperty<bool> IsAttacking => _isAttacking;

    private void Awake()
    {
        _info = GetComponent<PlayerInfo>();

        _info.Inputs.Player.Fire.started += Fire;
    }

    private void Fire(InputAction.CallbackContext callbackContext)
    {
        if (!_info.IsGrounded.Value) return; // 空中では攻撃しない

        _isAttacking.Value = true;
        _info.CanMove = false;
    }

    /// <summary>
    /// アニメーションイベントから呼び出す想定で作成したメソッド。<br/>
    /// 特定のエリア内にいる 被ダメージオブジェクトの Damage()を呼び出す。
    /// </summary>
    public void Attack()
    {
        var colls = _attackArea.GetColliders(transform);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(_info.AttackPower);
            }
        }
    }
    /// <summary>
    /// アニメーションイベントから呼び出す想定で作成したメソッド。<br/>
    /// 攻撃アニメーションの終了を検知する。
    /// </summary>
    public void AttackEnd()
    {
        _isAttacking.Value = false;
        _info.CanMove = true;
    }
}
