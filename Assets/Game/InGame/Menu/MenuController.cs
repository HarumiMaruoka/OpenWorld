// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuWindow = default;

    private void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            _menuWindow.SetActive(!_menuWindow.activeSelf);
        }
    }
}
