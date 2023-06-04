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

    public Transform Transform => transform;
    public IReadOnlyReactiveProperty<float> CurrentLife => _currentLife;

    private void Awake()
    {
        _currentLife.Value = _maxLife;
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
        _currentLife.Value -= value.AttackValue;

        if (_currentLife.Value < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
