using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor_RandomWalk : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField]
    private float _moveSpeed = 3f;
    [Header("回転速度")]
    [SerializeField]
    private float _rotationSpeed = 400f;
    [Header("方向転換時間")]
    [SerializeField]
    private float _changeDirectionInterval = 3f;
    [Header("壁検知用Rayの長さ")]
    [SerializeField]
    private float _raycastFrontDistance = 1f;
    [Header("崖検知用Rayの長さ")]
    [SerializeField]
    private float _raycastDownDistance = 1.5f;
    [Header("方向転換（再計算）用")]
    [SerializeField]
    private float _raycastOffsetAngle = 45f;
    [Header("壁のレイヤー")]
    [SerializeField]
    private LayerMask _obstacleLayer;

    private CharacterController _characterController;
    private float _currentDirection;
    private float _changeDirectionTimer;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _changeDirectionTimer = _changeDirectionInterval;
        RandomizeDirection();
    }

    private void Update()
    {
        //Move();
    }
    private void Move()
    {
        // 方向転換のタイマーを更新
        _changeDirectionTimer -= Time.deltaTime;
        if (_changeDirectionTimer <= 0f)
        {
            // 方向転換を行う
            RandomizeDirection();
            _changeDirectionTimer = _changeDirectionInterval;
        }

        // 移動処理
        Vector3 moveDirection = Quaternion.Euler(0f, _currentDirection, 0f) * Vector3.forward;

        // 崖の検知用Rayを足元に飛ばす
        Vector3 raycastOrigin = transform.position + Quaternion.Euler(0f, _currentDirection, 0f) * Vector3.forward * _raycastFrontDistance;
        RaycastHit groundHit;
        bool isOnGround = Physics.Raycast(raycastOrigin, Vector3.down, out groundHit, _raycastDownDistance);
        if (!isOnGround)
        {
            // Rayが何にも当たらない場合は再計算
            RandomizeDirection();
        }

        // 移動方向に障害物があるか判定
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, _raycastFrontDistance, _obstacleLayer))
        {
            // Rayが何かに当たった場合は再計算
            RandomizeDirection();
        }

        _characterController.SimpleMove(moveDirection * _moveSpeed);

        // プレイヤーの向きを移動方向に合わせる
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void RandomizeDirection()
    {
        // ランダムな角度を生成して現在の方向とする
        float randomAngle = Random.Range(0f, 360f);
        _currentDirection = randomAngle;
    }

#if UNITY_EDITOR
    [Header("ギズモを描画するかどうか")]
    [SerializeField]
    private bool _isDrawGizmos = true;
    private void OnDrawGizmosSelected()
    {
        if (!_isDrawGizmos) return;
        // GizmoにRayを描画する
        Gizmos.color = Color.red;
        Vector3 raycastOrigin = transform.position + Quaternion.Euler(0f, _currentDirection, 0f) * Vector3.forward * _raycastFrontDistance;
        Gizmos.DrawRay(raycastOrigin, Vector3.down * _raycastDownDistance);

        Gizmos.color = Color.blue;
        Vector3 moveDirection = Quaternion.Euler(0f, _currentDirection, 0f) * Vector3.forward;
        Gizmos.DrawRay(transform.position, moveDirection * _raycastFrontDistance);
    }
#endif
}