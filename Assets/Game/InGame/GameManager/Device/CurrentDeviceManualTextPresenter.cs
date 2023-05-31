// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CurrentDeviceManualTextPresenter : MonoBehaviour
{
    [Header("操作方法を表示するテキストコンポーネント")]
    [SerializeField]
    private Text _text = default;

    [Header("キーボードマウスの操作方法テキスト")]
    [Multiline, SerializeField]
    private string _keyboardMouseManualText = default;
    [Header("ゲームパッドの操作方法テキスト")]
    [Multiline, SerializeField]
    private string _gamePadManualText = default;

    private void Awake()
    {
        GameManager.Instance.DeviceManager.CurrentDevice.Subscribe(value =>
        {
            if (value == DeviceType.KeyboardAndMouse)
            {
                _text.text = _keyboardMouseManualText;
            }
            else if (value == DeviceType.GamePad)
            {
                _text.text = _gamePadManualText;
            }
        });
    }
}
