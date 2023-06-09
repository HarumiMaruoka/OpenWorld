using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

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
    }

    private void Update()
    {
        var moveDirection = (PlayerInfo.CurrentPlayerInfo.transform.position - transform.position);
        moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
        _characterController.SimpleMove(moveDirection * _moveSpeed * Time.deltaTime);

        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }
}