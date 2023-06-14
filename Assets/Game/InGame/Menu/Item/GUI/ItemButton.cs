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
    private ItemIsUsedWindow _isUsedWindow = default;
    private ItemButtonController _itemButtonController = default;
    private ItemButtonType _itemButtonType = default;

    public int MyItemID => _id;
    public Button Button => _button;
    public ItemButtonType ItemButtonType => _itemButtonType;

    public async void Setup(int id, ItemButtonController itemButtonController, ItemButtonType itemButtonType)
    {
        _id = id; _itemButtonController = itemButtonController; _itemButtonType = itemButtonType;
        _name = (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).Name;
        _nameText.text = _name;
        (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).InventoryCount.Subscribe(UpdateStockCountText);
        GetComponent<Button>().onClick.AddListener(UseItem);
    }
    public void SetIsUsedWindow(ItemIsUsedWindow isUsedWindow)
    {
        _isUsedWindow = isUsedWindow;
    }
    public void SetIcon(Sprite iconImage)
    {
        _iconImage.sprite = iconImage;
    }
    private void UpdateStockCountText(int number)
    {
        _stockNumberText.text = $"✕{number}";
    }
    private async void UseItem()
    {
        if (ItemManager.ShouldConfirmItemUsage)
        {
            // 確認ウィンドウを表示する
            _isUsedWindow.OpenWindow();
            _isUsedWindow.SetMessageText(_name);
            // 決定を待つ
            var result = await _isUsedWindow.WaitDecision(this.GetCancellationTokenOnDestroy());
            // 許可されなかった場合
            if (!result) return;
        }
        (await ItemManager.GetItemData(_id, this.GetCancellationTokenOnDestroy())).UseItem(PlayerInfo.CurrentPlayerInfo);
    }
    public void SetNavigation(Selectable up, Selectable down, Selectable right, Selectable left)
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

    public async void UpdateNavigation()
    {
        Navigation navigation = _button.navigation;
        navigation.mode = Navigation.Mode.Explicit;

        navigation.selectOnUp = await _itemButtonController.GetUpButton(this);
        navigation.selectOnDown = await _itemButtonController.GetDownButton(this);
        navigation.selectOnRight = _itemButtonController.GetRightButton(this);
        navigation.selectOnLeft = _itemButtonController.GetLeftButton(this);

        _button.navigation = navigation;
    }
}
