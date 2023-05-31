// 日本語対応
using UnityEngine;
using System;

namespace HitSupport
{
    [Serializable]
    public class SphereCast
    {
        [SerializeField]
        private Transform _origin = default;
        [SerializeField]
        private Vector3 _dir = default;
        [SerializeField]
        private float _radius = 0.5f;
        [SerializeField]
        private float _maxDistance = 100f;
        [SerializeField]
        private LayerMask _layerMask = default;

        private int _previousFrameCount = default;
        private RaycastHit _cachedHitInfo = default;

        public RaycastHit GetHitInfo()
        {
            if (_previousFrameCount != Time.frameCount)
            {
                if (_origin == null) return default;
                _previousFrameCount = Time.frameCount;
                Physics.SphereCast(_origin.position, _radius, _origin.rotation * _dir, out _cachedHitInfo, _maxDistance, _layerMask);
            }

            return _cachedHitInfo;
        }
        public bool IsHit(Transform transform)
        {
            return GetHitInfo().collider != null;
        }

#if UNITY_EDITOR
        [SerializeField]
        private bool _isDrawGizmos = false;
        [SerializeField]
        private Color _hitColor = Color.red;
        [SerializeField]
        private Color _noHitColor = Color.blue;
        public void OnDrawGizmos()
        {
            if (_isDrawGizmos == false)
                return;
            if (_origin == null)
                return;

            var hit = GetHitInfo();

            if (hit.collider != null)
            {
                Gizmos.color = _hitColor;
                Gizmos.DrawRay(_origin.position, _origin.rotation * _dir.normalized * hit.distance);
                Gizmos.DrawSphere(_origin.position + _origin.rotation * _dir.normalized * hit.distance, _radius);
            }
            else
            {
                Gizmos.color = _noHitColor;
                Gizmos.DrawRay(_origin.position, _origin.rotation * _dir.normalized * _maxDistance);
                Gizmos.DrawSphere(_origin.position + _origin.rotation * _dir.normalized * _maxDistance, _radius);
            }
        }
#endif
    }
}