// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// プレイヤーの回避を表現するクラス
/// </summary>
public class PlayerAvoidance : MonoBehaviour
{
    [SerializeField]
    private float _avoidanceTime = 1f;
    [SerializeField]
    private float _avoidanceInterval = 0.2f;

    private PlayerInfo _playerInfo = null;
    private PlayerMove _playerMove = null;
    private PlayerAttack _playerAttack = null;
    private PlayerWeaponController _playerWeaponController = default;

    public bool IsInInterval { get; private set; } = false;

    private BoolReactiveProperty _isAvoiding = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsAvoiding => _isAvoiding;

    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerInfo.Inputs.Player.Avoidance.started += Avoidance;
    }

    private async void Avoidance(InputAction.CallbackContext callbackContext)
    {
        if (!CanAvoidance()) return;

        _playerAttack.AttackEnd(); // （攻撃中であれば）攻撃をキャンセルする。
        _playerWeaponController.DeactivateWeapon(0);

        _playerMove.SetSpeed(float.MaxValue); // 初速度は最高速度

        _playerInfo.AddState(PlayerState.Avoidance);
        _isAvoiding.Value = true;
        await AvoidanceMove();
        _playerInfo.RemoveState(PlayerState.Avoidance);
        _isAvoiding.Value = false;

        _playerMove.Stop();

        StartInterval();
    }

    private bool CanAvoidance()
    {
        // ここに回避が実行可能かどうかを判定する処理を記述する。
        if (IsInInterval) return false; // インターバル中は無効
        if (_isAvoiding.Value) return false; // 既に回避中の場合は無効
        if (_playerInfo.CurrentState.Value.HasFlag(PlayerState.Talk)) return false; // 会話中は無効
        if (_playerInfo.CurrentState.Value.HasFlag(PlayerState.Midair)) return false; // 空中にいる間は無効
        if (_playerInfo.CurrentState.Value.HasFlag(PlayerState.Land)) return false; // 着地中は無効

        return true;
    }

    private async void StartInterval()
    {
        var startTime = Time.time;
        IsInInterval = true;
        await UniTask.Delay((int)(_avoidanceInterval * 1000f));
        IsInInterval = false;
    }
    private async UniTask AvoidanceMove()
    {
        // Debug.Log("回避開始");

        float currentTime = Time.time;

        while (!this.GetCancellationTokenOnDestroy().IsCancellationRequested && Time.time - currentTime < _avoidanceTime)
        {
            float inputThreshold = 0.1f;
            Vector3 moveDir;
            // 移動入力がある場合、その方向に回避する。
            Vector2 inputDir;
            if (Vector2.SqrMagnitude(inputDir = _playerInfo.Inputs.Player.Move.ReadValue<Vector2>()) > inputThreshold * inputThreshold)
            {
                moveDir = inputDir;
                _playerMove.Move(moveDir, true, false, true);
            }
            // 移動入力がない場合、プレイヤーが向いている方向に回避する。
            else
            {
                moveDir = transform.forward;
                //Debug.Log(transform.forward);
                moveDir = new Vector3(moveDir.x, moveDir.z, 0f).normalized; // _playerMove.Move()の都合で加工する。
                _playerMove.Move(moveDir, true, false, false);
            }

            await UniTask.Yield();
            if (this == null) return;
        }
    }
}
