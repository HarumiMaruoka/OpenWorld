// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private Button _button = default;
    [SerializeField]
    private Image _iconImage = default;
    [SerializeField]
    private Text _nameText = default;
    [SerializeField]
    private Text _stockNumberText = default;

    private int _id = default;
    private string _name = default;
    private ItemIsUsedWindow _checkItemIsUsedWindow = default;

    public int MyItemID => _id;

    public async void Setup(int id)
    {
        _id = id;
        _name = (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).Name;
        _nameText.text = _name;
        (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).InventoryCount.Subscribe(AssignStockCount);
        GetComponent<Button>().onClick.AddListener(UseItem);
    }
    public void SetCheckItemIsUsedWindow(ItemIsUsedWindow checkItemIsUsedWindow)
    {
        _checkItemIsUsedWindow = checkItemIsUsedWindow;
    }
    public void SetIcon(Sprite iconImage)
    {
        _iconImage.sprite = iconImage;
    }
    private void AssignStockCount(int number)
    {
        _stockNumberText.text = $"✕{number}";
    }
    private async void UseItem()
    {
        if (ItemManager.ShouldConfirmItemUsage)
        {
            // 確認ウィンドウを表示する
            _checkItemIsUsedWindow.OpenWindow();
            _checkItemIsUsedWindow.SetMessageText(_name);
            // 決定を待つ
            var result = await _checkItemIsUsedWindow.WaitDecision(this.GetCancellationTokenOnDestroy());
            // 許可されなかった場合
            if (!result) return;
        }
        (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).UseItem(PlayerInfo.CurrentPlayerInfo);
    }
    private void SetNavigation(Selectable up, Selectable down, Selectable right, Selectable left)
    {
        Navigation navigation = _button.navigation;

        navigation.mode = Navigation.Mode.Explicit;

        // 上の設定
        navigation.selectOnUp = up;
        // 下の設定
        navigation.selectOnDown = down;
        // 右を設定
        navigation.selectOnRight = right;
        // 左を設定
        navigation.selectOnLeft = left;

        _button.navigation = navigation;
    }
}
