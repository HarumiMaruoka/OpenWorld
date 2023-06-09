// 日本語対応
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

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
    public async void Fire(CancellationToken token)
    {
        await _meshRenderer.material.DOColor(_explosionColor, _explosionDelay).ToUniTask(cancellationToken: token);
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        // ここに攻撃処理を記述する
        if (PlayerLife.Current != null &&
            Vector3.SqrMagnitude(PlayerLife.Current.Transform.position - transform.position) < _attackRadius)
        {
            PlayerLife.Current.Damage(new AttackSet(default, _attackPower, default, default));
        }
        await _meshRenderer.material.DOColor(_nomalColor, _recoveryDelay).ToUniTask(cancellationToken: token);

        OnAttackCompleted?.Invoke();
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
