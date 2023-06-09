// 日本語対応
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

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
    [SerializeField]
    private float _landingTime = 1f;

    private CharacterController _characterController = null;
    private PlayerInfo _playerInfo = null;
    private PlayerAvoidance _playerAvoidance = null;

    private FloatReactiveProperty _currentHorizontalSpeed = new FloatReactiveProperty(0f);
    private FloatReactiveProperty _currentVerticalSpeed = new FloatReactiveProperty(0f);

    private Vector3 _cachedDir = default;
    private Quaternion _targetRotation = default;

    public IReadOnlyReactiveProperty<float> CurrentHorizontalSpeed => _currentHorizontalSpeed;
    public IReadOnlyReactiveProperty<float> CurrentVerticalSpeed => _currentVerticalSpeed;

    public float MaxHorizontalSpeed => _maxHorizontalSpeed;
    public float MaxVerticalSpeed => _maxVerticalSpeed;

    public bool CanRun
    {
        // 移動可能かどうかを判定する 条件式をまとめて記述するところ
        get
        {
            return
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Attack) && // 攻撃中は移動できない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Land) && // 着地中は移動できない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Talk) && // 会話中は移動できない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Avoidance); // 回避中は移動できない
        }
    }
    public bool CanJump
    {
        // ジャンプ可能かどうかを判定する 条件式をまとめて記述するところ
        get
        {
            return
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Attack) && // 攻撃中はジャンプできない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Land) && // 着地中はジャンプできない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Talk) && // 会話中はジャンプできない
                !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Avoidance) && // 回避中はジャンプできない
                !_playerAvoidance.IsInInterval; // 回避インターバル中はジャンプできない
        }
    }

    private bool IsMoveCalculate
    {
        get
        {
            return !_playerInfo.CurrentState.Value.HasFlag(PlayerState.Avoidance); // 回避中はMoveを実行しない。
        }
    }
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInfo = GetComponent<PlayerInfo>();
        _playerAvoidance = GetComponent<PlayerAvoidance>();
        _playerInfo.IsGrounded.Subscribe(value => { if (value) Landed(); }); // 着地を検知し、着地時処理を呼び出す。
    }

    private void Update()
    {
        if (IsMoveCalculate)
            Move(_playerInfo.Inputs.Player.Move.ReadValue<Vector2>(), CanRun, CanJump, true);
    }

    public void Move(Vector2 moveInput, bool canHorizontalMove, bool canJump, bool isCameraDir)
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
        // 接地している かつ ジャンプ入力があればジャンプする。着地中は無効
        else if (_playerInfo.Inputs.Player.Jump.IsPressed() && canJump)
        {
            _currentVerticalSpeed.Value = _playerInfo.JumpPower;
        }
        // 接地していれば速度は垂直速度は0。
        else
        {
            _currentVerticalSpeed.Value = 0f;
        }

        // 水平方向の制御
        if (moveInput.sqrMagnitude > 0.3f && canHorizontalMove)
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
            // メインカメラを基準に方向を指定する。
            if (isCameraDir)
            {
                _cachedDir = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * _cachedDir;
            }
            // 回転の制御
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
    [SerializeField]
    private float _landInterval = 0.6f;

    public bool IsLnadIntervalNow { get; private set; } = false;
    private async void Landed()
    {
        if (IsLnadIntervalNow) return;

        try
        {
            _playerInfo.AddState(PlayerState.Land);
            await UniTask.Delay((int)(_landingTime * 1000f), cancellationToken: this.GetCancellationTokenOnDestroy());
            IsLnadIntervalNow = true;
            _playerInfo.RemoveState(PlayerState.Land);
            await UniTask.Delay((int)(_landInterval * 1000f), cancellationToken: this.GetCancellationTokenOnDestroy());
            IsLnadIntervalNow = false;
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    public void Stop()
    {
        _currentHorizontalSpeed.Value = 0f;
    }
    public void SetSpeed(float value)
    {
        _currentHorizontalSpeed.Value = value;
    }
}
