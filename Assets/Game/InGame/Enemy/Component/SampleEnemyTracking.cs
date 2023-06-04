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
    private CancellationTokenSource _tokenSource = null;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// 追跡する
    /// </summary>
    public async void Tracking(Transform target, CancellationToken token)
    {
        _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

        Debug.Log("追跡開始");
        while (!_tokenSource.IsCancellationRequested)
        {
            if (!this.gameObject.activeSelf) return;
            var moveDirection = (target.position - transform.position);
            moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
            _characterController.SimpleMove(moveDirection * _moveSpeed * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            await UniTask.Yield();
        }

        Debug.Log("追跡終了");
    }

    /// <summary>
    /// 見失った
    /// </summary>
    public void LostSightOf()
    {
        if (_tokenSource != null)
        {
            _tokenSource.Cancel();
        }
    }
}