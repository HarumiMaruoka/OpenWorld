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

    public int MyItemID => _id;

    public async void Setup(int id)
    {
        _id = id;
        _nameText.text = (await ItemManager.GetItemData(this.GetCancellationTokenOnDestroy()))[_id].Name;
        (await ItemManager.GetItemData(this.GetCancellationTokenOnDestroy()))[_id].InventoryCount.Subscribe(AssignStockCount);
        GetComponent<Button>().onClick.AddListener(UseItem);
    }
    public void SetIcon(Sprite iconImage)
    {
        _iconImage.sprite = iconImage;
    }
    private void AssignStockCount(int number)
    {
        _stockNumberText.text = $"✕{number}";
    }
    public async void SetExplanatoryText(Text text)
    {
        text.text = (await ItemManager.GetItemData(this.GetCancellationTokenOnDestroy()))[_id].ExplanatoryText;
    }
    private async void UseItem()
    {
        (await ItemManager.GetItemData(this.GetCancellationTokenOnDestroy()))[_id].UseItem(PlayerInfo.CurrentPlayerInfo);
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
