// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ItemFillterButton : MonoBehaviour
{
    [SerializeField]
    private ItemButtonController _itemButtonController = default;
    [SerializeField]
    private ItemFilterButtonController _itemFilterButtonController = default;
    [SerializeField]
    private ItemButtonType _itemButtonType = default;
    [SerializeField]
    private Button _button = null;

    public Button Button => _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
    }
    private void OnClicked()
    {
        _itemFilterButtonController.ChangeFilterType(_itemButtonType);
        SelectTheTopButton();
    }
    public async void SelectTheTopButton()
    {
        // 上から下に向かって検索する。
        for (int i = 0; i < _itemButtonController.Buttons[_itemButtonType].Count; i++)
        {
            if ((await ItemManager.GetItemData(_itemButtonController.Buttons[_itemButtonType][i].MyItemID,
                this.GetCancellationTokenOnDestroy())).InventoryCount.Value > 0)
            {
                _itemButtonController.Buttons[_itemButtonType][i].Button.Select();
                return;
            }
        }
        // 検索にヒットするオブジェクトが見つからなければ自身を選択する。
        _button.Select();
    }
}
