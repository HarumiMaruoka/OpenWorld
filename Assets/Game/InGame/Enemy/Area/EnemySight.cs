// 日本語対応
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private Transform _eye;
    [SerializeField]
    private float _sightAngle;
    [SerializeField]
    private float _maxDistance = 10f;


    private HashSet<Transform> _targets = new HashSet<Transform>();

    public float SightAngle { get => _sightAngle; }
    public float MaxDistance { get => _maxDistance; }

    public void AddTarget(Transform target)
    {
        _targets.Add(target);
    }
    public void RemoveTarget(Transform target)
    {
        _targets.Remove(target);
    }
    public Transform SetNearTarget()
    {
        Transform result = null;
        float squaredDistance = float.MaxValue;
        foreach (var e in _targets)
        {
            if (result == null)
            {
                result = e;
                continue;
            }
            var tempSD = Mathf.Abs(result.position.sqrMagnitude - e.position.sqrMagnitude);
            if (squaredDistance > tempSD)
            {
                squaredDistance = tempSD;
                result = e;
                continue;
            }
        }
        return result;
    }

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible(Transform target)
    {
        if (_targets.Count == 0) return false;
        // 自身の向き（正規化されたベクトル）
        var selfDir = _eye.forward;
        // ターゲットまでの向きと距離計算
        var targetDir = target.position - _eye.position;
        var targetDistance = targetDir.magnitude;
        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);
        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);
        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }
#if UNITY_EDITOR
    [SerializeField]
    private bool _isDrawGizmo = false;
    [SerializeField]
    private Color _hitConeColor = Color.red;
    [SerializeField]
    private Color _noHitConeColor = Color.blue;
    private void OnDrawGizmosSelected()
    {
        if (!_isDrawGizmo) return;
        if (_eye == null) return;
        var selfPos = _eye.position;
        var selfDir = _eye.forward;

        // 円錐の頂点
        var coneApex = selfPos;
        // 円錐の底面円の半径
        var coneRadius = _maxDistance * Mathf.Tan(_sightAngle * 0.5f * Mathf.Deg2Rad);
        bool isFind = false;
        foreach (var e in _targets)
        {
            if (IsVisible(e))
            {
                isFind = true;
                break;
            }
        }
        // 円錐の底面円を描画
        DrawGizmosCone(coneApex, selfDir, coneRadius, isFind ? _hitConeColor : _noHitConeColor);
    }

    /// <summary> 円錐のギズモを描画する </summary>
    /// <param name="apex"> 頂点座標 </param>
    /// <param name="direction"> 前方方向 </param>
    /// <param name="radius"> 底面の半径 </param>
    /// <param name="color"> 描画する色 </param>
    private void DrawGizmosCone(Vector3 apex, Vector3 direction, float radius, Color color)
    {
        // 線の数
        const int segments = 64;
        // 円錐の高さ
        var height = _maxDistance;
        // 円錐の原点から見て円錐の底面の前方ベクトル
        var forward = direction.normalized;
        // 円錐の原点から見て右方向ベクトル
        var right = Vector3.Cross(Vector3.up, forward).normalized;
        // 円錐の原点から見て上方向ベクトル
        var up = Vector3.Cross(forward, right).normalized;
        // 底面（円）の中心座標
        var bottomCenter = apex + forward * height;
        // Gizmoの色を指定する
        Gizmos.color = color;
        // 放射状に線を描画する。底面円を描画。
        var lastPoint = bottomCenter + right * radius;
        for (int i = 0; i <= segments; i++)
        {
            // 角度を計算する
            var angleStep = 2f * Mathf.PI / segments;
            var currentAngle = angleStep * i;
            // 座標を計算する  円の中心座標 + 角度ベクトル * 半径
            var currentPoint = bottomCenter + (right * Mathf.Cos(currentAngle) + up * Mathf.Sin(currentAngle)) * radius;

            var currentPointForCircle = bottomCenter + (right * Mathf.Cos(currentAngle) + up * Mathf.Sin(currentAngle)) * radius;

            // 頂点から円の外周に向かって線を描画する
            Gizmos.DrawLine(apex, currentPoint);
            // 円の中心から外周に向かって線を描画する
            Gizmos.DrawLine(currentPoint, bottomCenter);

            Gizmos.DrawLine(lastPoint, currentPointForCircle);
            lastPoint = currentPointForCircle;
        }
    }
#endif
}