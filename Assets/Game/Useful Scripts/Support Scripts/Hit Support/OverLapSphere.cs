using UnityEngine;

namespace HitSupport
{
    [System.Serializable]
    public class OverLapSphere
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private LayerMask _targetLayer;

        private int _previousFrameCount = default;
        private Collider[] _cachedColliders = default;
        public bool IsFlipX { get; set; } = false;

        public Collider[] GetColliders(Transform origin)
        {
            if (_previousFrameCount != Time.frameCount)
            {
                _previousFrameCount = Time.frameCount;
                var offset = _offset;
                if (IsFlipX)
                {
                    offset.x *= -1;
                }
                var dir = origin.rotation * offset;
                return _cachedColliders = Physics.OverlapSphere(origin.position + dir, _radius, _targetLayer);
            }
            else
            {
                return _cachedColliders;
            }
        }

        public bool IsHit(Transform origin)
        {
            var colls = GetColliders(origin);
            if (colls == null) return false;
            return colls.Length > 0;
        }

#if UNITY_EDITOR
        [Header("Gizmo関連")]
        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _hitColor = Color.red;
        [SerializeField]
        private Color _noHitColor = Color.blue;

        public void OnDrawGizmos(Transform origin)
        {
            if (_isDrawGizmo)
            {
                var offset = _offset;
                if (IsFlipX)
                {
                    offset.x *= -1;
                }
                var dir = origin.rotation * offset;

                if (IsHit(origin))
                {
                    Gizmos.color = _hitColor;
                }
                else
                {
                    Gizmos.color = _noHitColor;
                }
                Gizmos.DrawSphere(origin.position + dir, _radius);
            }
        }
#endif
    }
}