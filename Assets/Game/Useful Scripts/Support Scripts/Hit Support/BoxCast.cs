// 日本語対応
using UnityEngine;
using System;

namespace HitSupport
{
    [Serializable]
    public class BoxCast
    {
        [SerializeField]
        private Vector3 _dir = default;
        [SerializeField]
        private Vector3 _halfExtents = default;
        [SerializeField]
        private float _maxDistance = 100f;
        [SerializeField]
        private LayerMask _layerMask = default;

        private int _previousFrameCount = default;
        private RaycastHit _cachedHitInfo = default;

        public RaycastHit GetHitInfo(Transform transform)
        {
            if (_previousFrameCount != Time.frameCount)
            {
                _previousFrameCount = Time.frameCount;
                Physics.BoxCast(transform.position, _halfExtents, transform.rotation * _dir, out _cachedHitInfo, transform.rotation, _maxDistance, _layerMask);
            }
            return _cachedHitInfo;
        }
        public bool IsHit(Transform transform)
        {
            return GetHitInfo(transform).collider != null;
        }

#if UNITY_EDITOR
        [SerializeField]
        private bool _isDrawGizmos = false;
        [SerializeField]
        private Color _hitColor = Color.red;
        [SerializeField]
        private Color _noHitColor = Color.blue;
        public void OnDrawGizmos(Transform transform)
        {
            if (_isDrawGizmos == false)
                return;

            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            var hit = GetHitInfo(transform);
            if (hit.collider != null)
            {
                Gizmos.color = _hitColor;
                Gizmos.DrawRay(Vector3.zero, _dir.normalized * hit.distance);
                Gizmos.DrawCube(Vector3.zero + _dir.normalized * hit.distance, _halfExtents * 2f);
            }
            else
            {
                Gizmos.color = _noHitColor;
                Gizmos.DrawRay(Vector3.zero, _dir.normalized * _maxDistance);
                Gizmos.DrawCube(Vector3.zero + _dir.normalized * _maxDistance, _halfExtents * 2f);
            }
            Gizmos.matrix = oldMatrix;
        }
#endif
    }
}