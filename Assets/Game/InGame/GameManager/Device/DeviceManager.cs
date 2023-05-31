// 日本語対応
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
//
public class DeviceManager
{
    private ReactiveProperty<DeviceType> _currentDevice =
        new ReactiveProperty<DeviceType>(DeviceType.KeyboardAndMouse);

    public ReactiveProperty<DeviceType> CurrentDevice => _currentDevice;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private CancellationToken _cancellationToken = default;

    public DeviceManager()
    {
        Debug.Log("DeviceManagerのインスタンスを確保");
        Start();
    }
    ~DeviceManager()
    {
        Debug.Log("DeviceManagerのインスタンスを破棄");
        Stop();
    }

    private async void Start()
    {
        _cancellationToken = _cancellationTokenSource.Token;
        while (!_cancellationToken.IsCancellationRequested)
        {
            // キーボード,マウスの入力を監視する
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current != null && (
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame ||
            Mouse.current.middleButton.wasPressedThisFrame ||
            Mouse.current.forwardButton.wasPressedThisFrame ||
            Mouse.current.backButton.wasPressedThisFrame))
            {
                _currentDevice.Value = DeviceType.KeyboardAndMouse;
            }
            // ゲームパッドの入力を監視する
            else if (Gamepad.current != null && (
            // 〇△□×,ABCDボタン
            Gamepad.current.buttonSouth.wasPressedThisFrame ||
            Gamepad.current.buttonNorth.wasPressedThisFrame ||
            Gamepad.current.buttonWest.wasPressedThisFrame ||
            Gamepad.current.buttonEast.wasPressedThisFrame ||
            // スティック押し込み
            Gamepad.current.leftStickButton.wasPressedThisFrame ||
            Gamepad.current.rightStickButton.wasPressedThisFrame ||
            // スタート、セレクトボタン
            Gamepad.current.selectButton.wasPressedThisFrame ||
            Gamepad.current.startButton.wasPressedThisFrame ||
            // 十字キー
            Gamepad.current.dpad.left.wasPressedThisFrame ||
            Gamepad.current.dpad.right.wasPressedThisFrame ||
            Gamepad.current.dpad.up.wasPressedThisFrame ||
            Gamepad.current.dpad.down.wasPressedThisFrame ||
            // RB,LB,RT,LT
            Gamepad.current.rightShoulder.wasPressedThisFrame ||
            Gamepad.current.leftShoulder.wasPressedThisFrame ||
            Gamepad.current.rightTrigger.wasPressedThisFrame ||
            Gamepad.current.leftTrigger.wasPressedThisFrame ||
            // スティック傾き
            Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.8f ||
            Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.8f))
            {
                _currentDevice.Value = DeviceType.GamePad;
            }
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
    private void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
}
public enum DeviceType
{
    KeyboardAndMouse, GamePad
}
