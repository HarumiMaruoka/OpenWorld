// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OnActivationItemWindow : MonoBehaviour
{
    [SerializeField]
    private ItemButtonController _itemButtonController = default;
    [SerializeField]
    private ItemFillterButton _firstSelectedButton = default;

    private async void OnEnable()
    {
        await UniTask.WaitUntil(_itemButtonController.IsSetuped);
        _firstSelectedButton.Button.Select();
        _firstSelectedButton.SelectTheTopButton();
    }
}
