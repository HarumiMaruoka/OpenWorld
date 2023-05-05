using System;
using UnityEngine;

namespace HitSupport
{
    [System.Serializable]
    public class Raycast
    {
        [SerializeField]
        private Vector3 _dir = default;
        [SerializeField]
        private float _maxDistance = default;
        [SerializeField]
        private LayerMask _targetLayer = default;
        [SerializeField]
        private Vector3 _offset = Vector3.zero;

        private Transform _origin = null;

        private int _previousFrameCount = default;
        private RaycastHit _cachedRaycastHit = default;
        private bool _cachedIsHit = default;

        public void Init(Transform origin)
        {
            _origin = origin;
        }

        public bool IsHit()
        {
            if (_previousFrameCount != Time.frameCount)
            {
                _previousFrameCount = Time.frameCount;
                return _cachedIsHit = Physics.Raycast(_origin.position + _offset, _origin.rotation * _dir, _maxDistance, _targetLayer);
            }
            else
            {
                return _cachedIsHit;
            }
        }
        public bool IsHit(out RaycastHit result)
        {
            if (_previousFrameCount != Time.frameCount)
            {
                _previousFrameCount = Time.frameCount;
                var isHit = Physics.Raycast(_origin.position + _offset, _origin.rotation * _dir, out result, _maxDistance, _targetLayer);
                _cachedRaycastHit = result;
                return _cachedIsHit = isHit;
            }
            else
            {
                result = _cachedRaycastHit;
                return _cachedIsHit;
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _hitColor = Color.red;
        [SerializeField]
        private Color _noHitColor = Color.blue;
        public void OnDrawGizmo(Transform origin)
        {
            if (_isDrawGizmo)
            {
                // Rayがヒットした場合で色を変える。
                RaycastHit hit;
                if (Physics.Raycast(origin.position + _offset, origin.rotation * _dir, out hit, _maxDistance, _targetLayer))
                {
                    //衝突時のRayを画面に表示
                    Debug.DrawRay(
                        origin.position + _offset, // 開始位置
                        hit.point - (origin.position + _offset), //Rayの方向と距離
                        _hitColor, // ヒットした場合の色
                        0, // ラインを表示する時間（秒単位）
                        false); // ラインがカメラから近いオブジェクトによって隠された場合にラインを隠すかどうか
                    return;
                }

                //非衝突時のRayを画面に表示
                Debug.DrawRay(
                    origin.position + _offset,
                   (origin.rotation * _dir).normalized * _maxDistance,
                    _noHitColor,
                    0,
                    false);
            }
        }
#endif
    }
}