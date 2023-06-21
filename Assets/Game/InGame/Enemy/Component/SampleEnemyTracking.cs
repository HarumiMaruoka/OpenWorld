using UnityEngine;
using UniRx;

/// <summary>
/// Sample enemy の追跡状態を表現する。
/// 特定のキャラクターを追跡する
/// </summary>
public class SampleEnemyTracking : MonoBehaviour
{
    [Header("回転速度")]
    [SerializeField]
    private float _rotationSpeed = 400f;
    [Header("移動速度")]
    [SerializeField]
    private float _moveSpeed = 5f;

    private CharacterController _characterController = null;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        TimeStopManager.IsTimeStop.Subscribe(TimeStop).AddTo(this);
    }

    private bool _isTimeStop = false;

    private void Update()
    {
        if (_isTimeStop) return;

        var moveDirection = (PlayerInfo.CurrentPlayerInfo.transform.position - transform.position);
        moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
        _characterController.SimpleMove(moveDirection * _moveSpeed);

        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }

    private void TimeStop(bool value)
    {
        _isTimeStop = value;
    }
}