// 日本語対応
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UniRx;

/// <summary>
/// Sample enemy の攻撃状態を表現する。
/// 指定位置,範囲に向かって攻撃する。
/// </summary>
public class SampleEnemyAttack : MonoBehaviour
{
    [Header("攻撃関連")]
    [SerializeField]
    private float _attackPower = 3f;
    [SerializeField]
    private float _attackRadius = 5f;

    [Header("アニメーション関連")]
    [SerializeField]
    private float _explosionDelay = 1f;
    [SerializeField]
    private Color _explosionColor = Color.red;
    [SerializeField]
    private float _recoveryDelay = 1f;
    [SerializeField]
    private Color _nomalColor = Color.white;

    [SerializeField]
    private GameObject _explosionEffectPrefab = default;

    private MeshRenderer _meshRenderer = null;

    public event Action OnAttackCompleted = default;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnDisable()
    {
        _currentAnim?.Kill();
    }

    public void Fire()
    {
        if (TimeStopManager.IsTimeStop.Value) { OnAttackCompleted?.Invoke(); return; }
        StartFireAnim(() => this.Attack(() => this.EndFireAnim(() => OnAttackCompleted?.Invoke())));
    }

    TweenerCore<Color, Color, ColorOptions> _currentAnim = null;
    IDisposable _disposable = null;

    private void StartFireAnim(Action onComplete = null)
    {
        _disposable = TimeStopManager.IsTimeStop.Subscribe(value => { if (value) Pause(); else Resume(); });
        _currentAnim = _meshRenderer.material.DOColor(_explosionColor, _explosionDelay).
            OnComplete(() => { _currentAnim = null; onComplete?.Invoke(); });
    }
    private void Attack(Action onComplete = null)
    {
        if (this == null) return;
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        if (PlayerLife.Current != null &&
               Vector3.SqrMagnitude(PlayerLife.Current.Transform.position - transform.position) < _attackRadius)
        {
            PlayerLife.Current.Damage(new AttackSet(default, _attackPower, default, default));
        }
        onComplete?.Invoke();
    }
    private void EndFireAnim(Action onComplete = null)
    {
        _currentAnim = _meshRenderer.material.DOColor(_nomalColor, _recoveryDelay).
            OnComplete(() =>
                {
                    _currentAnim = null; onComplete?.Invoke();
                    _disposable.Dispose(); _disposable = null;
                });
    }

    private void Pause()
    {
        if (_currentAnim != null)
        {
            _currentAnim.Pause();
        }
    }
    private void Resume()
    {
        if (_currentAnim != null)
        {
            _currentAnim.Play();
        }
    }

#if UNITY_EDITOR
    [Header("Gizmo関連")]
    [SerializeField]
    private bool _isDrawGizmos = true;
    [SerializeField]
    private Color _gizmoColor = Color.blue;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _attackRadius);
    }
#endif
}
