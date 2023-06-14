// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonController : MonoBehaviour
{
    [SerializeField]
    private Button _allButton = default;
    [SerializeField]
    private Text _itemExplanatoryText = default;
    [SerializeField]
    private ItemButtonGenerator _buttonGgenerator = default;

    private Dictionary<ItemButtonType, List<ItemButton>> _buttons = new Dictionary<ItemButtonType, List<ItemButton>>();

    public IReadOnlyDictionary<ItemButtonType, List<ItemButton>> Buttons => _buttons;

    public bool _isSetuped = false;
    public bool IsSetuped() { return _isSetuped; }

    private async void Awake()
    {
        _buttonGgenerator.Generate(await ItemManager.GetItemDataAll(this.gameObject.GetCancellationTokenOnDestroy()));

        _buttons.Add(ItemButtonType.All, _buttonGgenerator.AllButtons);
        _buttons.Add(ItemButtonType.Healing, _buttonGgenerator.HealingButtons);
        _buttons.Add(ItemButtonType.Attack, _buttonGgenerator.AttackButtons);
        _buttons.Add(ItemButtonType.Support, _buttonGgenerator.SupportButtons);
        _buttons.Add(ItemButtonType.Valuables, _buttonGgenerator.ValuablesButtons);

        foreach (var e in _buttons)
            foreach (var item in e.Value)
                item.UpdateNavigation();

        _isSetuped = true;
    }

    private void OnEnable()
    {
        UIManager.Current.OnChangedSelectedObject += OnChangedSelectedObject;
    }
    private void OnDisable()
    {
        if (UIManager.Current != null)
            UIManager.Current.OnChangedSelectedObject -= OnChangedSelectedObject;
    }
    /// <summary> UIで選択しているオブジェクトが変更されたら、説明文を更新する。 </summary>
    /// <param name="oldObj"> 変更前のオブジェクト </param>
    /// <param name="newObj"> 変更後のオブジェクト </param>
    private async void OnChangedSelectedObject(GameObject oldObj, GameObject newObj)
    {
        if (newObj != null &&
            newObj.TryGetComponent(out ItemButton itemButton))
        {
            _itemExplanatoryText.text = (await ItemManager.GetItemData(itemButton.MyItemID, this.GetCancellationTokenOnDestroy())).ExplanatoryText;
        }
        else
        {
            _itemExplanatoryText.text = null;
        }
    }

    public async UniTask<Selectable> GetUpButton(ItemButton itemButton)
    {
        var buttons = _buttons[itemButton.ItemButtonType];
        int myIndex = buttons.IndexOf(itemButton);
        for (int i = myIndex - 1; i > -buttons.Count - myIndex; i--)
        {
            int index = i > -1 ? i : i + buttons.Count;
            var itemData = await ItemManager.GetItemData(buttons[index].MyItemID, this.GetCancellationTokenOnDestroy());
            if (itemData.InventoryCount.Value > 0)
            {
                return buttons[index].Button;
            }
        }
        return itemButton.Button;
    }
    public async UniTask<Selectable> GetDownButton(ItemButton itemButton)
    {
        var buttons = _buttons[itemButton.ItemButtonType];
        int myIndex = buttons.IndexOf(itemButton);
        for (int i = myIndex + 1; i < buttons.Count + myIndex; i++)
        {
            var itemData = await ItemManager.GetItemData(buttons[i % buttons.Count].MyItemID, this.GetCancellationTokenOnDestroy());
            if (itemData.InventoryCount.Value > 0)
            {
                return buttons[i % buttons.Count].Button;
            }
        }
        return itemButton.Button;
    }
    public Selectable GetRightButton(ItemButton itemButton)
    {
        return itemButton.Button;
    }
    public Selectable GetLeftButton(ItemButton itemButton)
    {
        return _allButton;
    }
}
