// 日本語対応
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInfo))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _gravity = 9.8f;
    [SerializeField]
    private float _rotationSpeed = 600f;
    [SerializeField]
    private float _maxHorizontalSpeed = 0f;
    [SerializeField]
    private float _maxVerticalSpeed = 0f;

    private CharacterController _characterController = null;
    private PlayerInfo _playerInfo = null;

    private FloatReactiveProperty _currentHorizontalSpeed = new FloatReactiveProperty(0f);
    private FloatReactiveProperty _currentVerticalSpeed = new FloatReactiveProperty(0f);

    private Vector3 _cachedDir = default;
    private Quaternion _targetRotation = default;

    public IReadOnlyReactiveProperty<float> CurrentHorizontalSpeed => _currentHorizontalSpeed;
    public IReadOnlyReactiveProperty<float> CurrentVerticalSpeed => _currentVerticalSpeed;

    public float MaxHorizontalSpeed => _maxHorizontalSpeed;
    public float MaxVerticalSpeed => _maxVerticalSpeed;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        Move(_playerInfo.Inputs.Player.Move.ReadValue<Vector2>());
    }

    private void Move(Vector2 moveInput)
    {
        // 垂直方向の制御
        // 接地してなければ落下する
        if (!_playerInfo.IsGrounded.Value)
        {
            _currentVerticalSpeed.Value -= _gravity * Time.deltaTime;
            if (_currentVerticalSpeed.Value < -_maxVerticalSpeed)
            {
                _currentVerticalSpeed.Value = -_maxVerticalSpeed;
            }
        }
        // 接地している かつ ジャンプ入力があればジャンプする。
        else if (_playerInfo.Inputs.Player.Jump.IsPressed())
        {
            _currentVerticalSpeed.Value = _playerInfo.JumpPower;
        }
        // 接地していれば速度は垂直速度は0。
        else
        {
            _currentVerticalSpeed.Value = 0f;
        }

        // 水平方向の制御
        if (moveInput.sqrMagnitude > 0.3f && _playerInfo.CanMove)
        {
            // 入力がある限り加速する
            _currentHorizontalSpeed.Value +=
                _playerInfo.GroundMoveSpeed * Time.deltaTime;
            if (_currentHorizontalSpeed.Value > _maxHorizontalSpeed)
            {
                _currentHorizontalSpeed.Value = _maxHorizontalSpeed;
            }

            // 入力方向を保存する
            _cachedDir = new Vector3(moveInput.x, 0f, moveInput.y);

            // 回転の制御
            // メインカメラを基準に方向を指定する。
            _cachedDir = Camera.main.transform.TransformDirection(_cachedDir);
            _targetRotation = Quaternion.LookRotation(_cachedDir, Vector3.up);
            _targetRotation.x = 0f;
            _targetRotation.z = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
        // 入力がなければ減速する
        else
        {
            _currentHorizontalSpeed.Value -=
                _playerInfo.GroundDeceleration * Time.deltaTime;

            if (_currentHorizontalSpeed.Value < 0f)
            {
                _currentHorizontalSpeed.Value = 0f;
            }
        }
        // 結果の割り当て
        Vector3 moveSpeed = _cachedDir.normalized * _currentHorizontalSpeed.Value * Time.deltaTime;
        moveSpeed.y = _currentVerticalSpeed.Value * Time.deltaTime;

        _characterController.Move(moveSpeed);
    }
}
