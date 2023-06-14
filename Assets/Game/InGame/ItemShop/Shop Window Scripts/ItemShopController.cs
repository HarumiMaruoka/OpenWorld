// 日本語対応
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;

public class ItemShopController : MonoBehaviour
{
    [Header("アイテムの購入確認ウィンドウ")]
    [SerializeField]
    private ItemBuyConfirmation _itemBuyConfirmation = default;
    [SerializeField]
    private Text _fundsText = default;
    [Header("このアイテムショップの売り物")]
    [SerializeField]
    private List<Product> _products = default;

    public IReadOnlyList<Product> Products => _products;
    public ItemBuyConfirmation ItemBuyConfirmationWindow => _itemBuyConfirmation;

    private void Start()
    {
        PlayerMoneyController.Current.Funds.Subscribe(value =>
        {
            _fundsText.text = $"{value.ToString("#,0")} 円";
        });
    }

    private void OnEnable()
    {
        // 上のやつを選択する
    }

    public void Buy(Product product)
    {
        _products.Remove(product);
    }

    /// <summary> 売り物を表す構造体。 </summary>
    [Serializable]
    public struct Product
    {
        [SerializeField]
        private int _itemID;
        [SerializeField]
        private long _cost;

        public int ItemID => _itemID;
        public long Cost => _cost;
    }
}
