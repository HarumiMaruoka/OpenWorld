// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonGenerator : MonoBehaviour
{
    [SerializeField]
    private ItemButtonController _itemButtonController = default;
    [Header("生成するアイテムボタンのプレハブ")]
    [SerializeField]
    private ItemButton _itemButtonPrefab = default;

    [Header("全アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _allParent = default;
    [Header("回復アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _healingParent = default;
    [Header("攻撃アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _attackParent = default;
    [Header("サポートアイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _supportParent = default;
    [Header("貴重品アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _valuablesParent = default;

    [Header("以下 アイテムボタンに渡す値")]
    [Header("アイテムを使用する際の確認ウィンドウ")]
    [SerializeField]
    private ItemIsUsedWindow _isUsedWindow = default;

    private List<ItemButton> _allButtons = new List<ItemButton>();
    private List<ItemButton> _healingButtons = new List<ItemButton>();
    private List<ItemButton> _attackButtons = new List<ItemButton>();
    private List<ItemButton> _supportButtons = new List<ItemButton>();
    private List<ItemButton> _valuablesButtons = new List<ItemButton>();

    public List<ItemButton> AllButtons => _allButtons;
    public List<ItemButton> HealingButtons => _healingButtons;
    public List<ItemButton> AttackButtons => _attackButtons;
    public List<ItemButton> SupportButtons => _supportButtons;
    public List<ItemButton> ValuablesButtons => _valuablesButtons;

    public void Generate(Item[] items, Action onComplete = null)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var button = Instantiate(_itemButtonPrefab, _allParent);
            button.Setup(i, _itemButtonController, ItemButtonType.All);
            button.SetIsUsedWindow(_isUsedWindow);
            _allButtons.Add(button);

            ItemButton itemButton;
            switch (items[i].EffectType)
            {
                case ItemEffectType.Healing:
                    itemButton = Instantiate(_itemButtonPrefab, _healingParent);
                    itemButton.Setup(i, _itemButtonController, ItemButtonType.Healing);
                    itemButton.SetIsUsedWindow(_isUsedWindow);
                    _healingButtons.Add(itemButton);
                    break;
                case ItemEffectType.Attack:
                    itemButton = Instantiate(_itemButtonPrefab, _attackParent);
                    itemButton.Setup(i, _itemButtonController, ItemButtonType.Attack);
                    itemButton.SetIsUsedWindow(_isUsedWindow);
                    _attackButtons.Add(itemButton);
                    break;
                case ItemEffectType.Support:
                    itemButton = Instantiate(_itemButtonPrefab, _supportParent);
                    itemButton.Setup(i, _itemButtonController, ItemButtonType.Support);
                    itemButton.SetIsUsedWindow(_isUsedWindow);
                    _supportButtons.Add(itemButton);
                    break;
                case ItemEffectType.Valuables:
                    itemButton = Instantiate(_itemButtonPrefab, _valuablesParent);
                    itemButton.Setup(i, _itemButtonController, ItemButtonType.Valuables);
                    itemButton.SetIsUsedWindow(_isUsedWindow);
                    _valuablesButtons.Add(itemButton);
                    break;
            }
        }
        onComplete?.Invoke();
    }
}
