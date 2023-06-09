// 日本語対応
using UnityEngine;
using UniRx;

public class PlayerLife : MonoBehaviour, IDamageable
{
    private static PlayerLife _current = null;
    public static PlayerLife Current => _current;

    [SerializeField]
    private float _maxLife = 10f;
    [SerializeField]
    private FloatReactiveProperty _currentLife;
    [SerializeField]
    private bool _isGodMode = false;

    private PlayerInfo _playerInfo = null;
    public Transform Transform => transform;
    public IReadOnlyReactiveProperty<float> CurrentLife => _currentLife;

    private void Awake()
    {
        _currentLife.Value = _maxLife;
        _playerInfo = GetComponent<PlayerInfo>();
    }
    private void OnEnable()
    {
        _current = this;
        DamageableManager.Register(this);
    }
    private void OnDisable()
    {
        _current = null;
        DamageableManager.Lift(this);
    }

    public void Damage(AttackSet value)
    {
        if (_isGodMode) return; // 無敵中はダメージを無視する。
        if (_playerInfo.CurrentState.Value.HasFlag(PlayerState.Avoidance)) return; // 回避中もダメージを無視する。

        _currentLife.Value -= value.AttackValue;

        if (_currentLife.Value < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
