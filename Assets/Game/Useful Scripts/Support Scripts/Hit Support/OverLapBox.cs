using UnityEngine;

namespace HitSupport
{
    [System.Serializable]
    public class OverLapBox
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _size;
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
                    offset *= -1f;
                }
                var dir = origin.rotation * offset; // 回転を考慮した本当のオフセットを取得
                return _cachedColliders = Physics.OverlapBox(origin.position + dir, _size / 2f, origin.rotation, _targetLayer);
            }
            else
            {
                return _cachedColliders;
            }
        }

        public bool IsHit(Transform origin)
        {
            return GetColliders(origin).Length > 0;
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
                if (IsHit(origin))
                {
                    Gizmos.color = _hitColor;
                }
                else
                {
                    Gizmos.color = _noHitColor;
                }

                var offset = _offset;
                if (IsFlipX)
                {
                    offset *= -1f;
                }

                //Gizmo はワールド座標指定なので、相対座標指定の場合はマトリクス変換で移動する
                Gizmos.matrix = Matrix4x4.TRS(origin.position, origin.rotation, Vector3.one);
                Gizmos.DrawCube(offset, _size);
                Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            }
        }
#endif
    }
}