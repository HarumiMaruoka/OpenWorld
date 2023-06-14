// 日本語対応
using System.Collections.Generic;
using UnityEngine;

public class ItemShopProductButtonGenerator : MonoBehaviour
{
    [Header("ボタンの親オブジェクト")]
    [SerializeField]
    private Transform _buttonParent = default;
    [Header("ショップボタンのプレハブ")]
    [SerializeField]
    private ItemShopProductButton _itemShopProductButtonPrefab = default;

    [SerializeField]
    private ItemShopController _itemShopController = default;


    private void Awake()
    {
        GenerateItemShopProductButton();
    }
    public void GenerateItemShopProductButton()
    {
        for (int i = 0; i < _itemShopController.Products.Count; i++)
        {
            Instantiate(_itemShopProductButtonPrefab, _buttonParent).Setup(_itemShopController, _itemShopController.Products[i]);
        }
    }
}
