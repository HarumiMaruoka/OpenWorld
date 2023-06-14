// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class ItemShopProductButton : MonoBehaviour
{
    [SerializeField]
    private Button _button = default;
    [SerializeField]
    private Image _icon = default;
    [SerializeField]
    private Text _name = default;
    [SerializeField]
    private Text _cost = default;

    private ItemShopController.Product _product;
    private ItemShopController _owner = null;

    private Item _itemData;

    public ItemShopController.Product Product => _product;

    public async void Setup(ItemShopController owner, ItemShopController.Product product)
    {
        _product = product; _owner = owner;

        _itemData = await ItemManager.GetItemData(product.ItemID, this.GetCancellationTokenOnDestroy());

        _button.onClick.AddListener(OnClick);
        _name.text = _itemData.Name;
        _cost.text = $"{product.Cost.ToString("#,0")} 円";
    }

    private async void OnClick()
    {
        // お金が足りるかどうか判定する
        if (PlayerMoneyController.Current.Funds.Value < _product.Cost)
        {
            // お金が足りない場合は お金が足りない事を表現しリターンする。
            //Debug.LogError("お金が足りないよ");

            return;
        }

        // お金が足りる場合、
        // 購入するかどうかのウィンドウを表示する。
        _owner.ItemBuyConfirmationWindow.Text.text = $"{_itemData.Name} を\n使用しますか？";
        _owner.ItemBuyConfirmationWindow.CurrentStockText.text = $"現在の所持数は {_itemData.InventoryCount.Value} 個 です。";
        _owner.ItemBuyConfirmationWindow.gameObject.SetActive(true);
        //Debug.LogError("お金足りるよ");
        // YesかNoボタンどっちか押すまで待つ。
        try
        {
            await UniTask.WaitUntil(_owner.ItemBuyConfirmationWindow.IsJudged, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        catch (OperationCanceledException)
        {
            return;
        }
        // Yesボタンを押した場合。
        if (_owner.ItemBuyConfirmationWindow.IsBuy)
        {
            // 購入処理
            PlayerMoneyController.Current.TryDeductMoney(_product.Cost);
            _itemData.IncrementInventoryCount();
            _owner.Buy(_product);
            this.gameObject.SetActive(false);
            Debug.Log("購入！");
        }
        // Noボタンを押した場合。
        else if (_owner.ItemBuyConfirmationWindow.IsNotBuy)
        {
            // キャンセル処理
            // なにもしない
            Debug.Log("キャンセル！");
        }
        // 後処理
        _owner.ItemBuyConfirmationWindow.gameObject.SetActive(false);
        _owner.ItemBuyConfirmationWindow.Clear();
    }
}
