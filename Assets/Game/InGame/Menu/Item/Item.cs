// 日本語対応
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary> アイテムの基底クラス </summary>
public class Item
{
    /// <summary> 所持数 </summary>
    [SerializeField] // jsonで保存するために左記の属性を付与する
    private readonly IntReactiveProperty _inventoryCount = new IntReactiveProperty(0);
    /// <summary> このアイテムのID </summary>
    private readonly int _id = -1;
    /// <summary> このアイテムの名前 </summary>
    private readonly string _name = null;
    /// <summary> このアイテムの説明文 </summary>
    private readonly string _explanatoryText = null;
    /// <summary> このアイテムの効果対象 </summary>
    private readonly ItemUseTargetType _targetType = default;
    /// <summary> このアイテムの効果 </summary>
    private readonly ItemEffectType _effectType = default;
    /// <summary> このアイテムの効果対象のリスト </summary>
    private readonly List<IItemEffectReceiver> _itemEffectReceivers = new List<IItemEffectReceiver>();
    /// <summary> このアイテムの効果対象を取捨選択する用のクラス </summary>
    private readonly ItemEffectFilterBase _itemEffectFilter = default;
    /// <summary> アイテムの所持数を保存する用のキー </summary>
    private const string PlayerPrefsInventoryCountKey = "InventoryCount";

    /// <summary> このアイテムの所持数 </summary>
    public IReadOnlyReactiveProperty<int> InventoryCount => _inventoryCount;
    /// <summary> このアイテムのID </summary>
    public int ID => _id;
    /// <summary> このアイテムの名前 </summary>
    public string Name => _name;
    /// <summary> このアイテムの説明文 </summary>
    public string ExplanatoryText => _explanatoryText;
    /// <summary> このアイテムの効果対象 </summary>
    public ItemUseTargetType TargetType => _targetType;
    /// <summary> このアイテムの効果 </summary>
    public ItemEffectType EffectType => _effectType;
    /// <summary> このアイテムの効果対象 </summary>
    public IReadOnlyList<IItemEffectReceiver> ItemEffectReceivers => _itemEffectReceivers;

    public Item(int id, string name, string explanatoryText,
         ItemEffectType effectType, ItemUseTargetType targetType, ItemEffectFilterBase itemEffectFilter)
    {
        _id = id; _name = name; _explanatoryText = explanatoryText;
        _effectType = effectType; _targetType = targetType;
        _itemEffectFilter = itemEffectFilter;
    }
    public void SaveInventoryCount()
    {
        PlayerPrefs.SetInt($"{PlayerPrefsInventoryCountKey}{_id}", _inventoryCount.Value);
    }
    public void LoadInventoryCount()
    {
        _inventoryCount.Value = PlayerPrefs.GetInt($"{PlayerPrefsInventoryCountKey}{_id}", 0);
    }
    public void SetInventoryCount(int inventoryCount)
    {
        if (inventoryCount <= 0) inventoryCount = 0;
        _inventoryCount.Value = inventoryCount;
    }
    public void IncrementInventoryCount()
    {
        _inventoryCount.Value++;
    }
    public void DecrementInventoryCount()
    {
        _inventoryCount.Value--;
        if (_inventoryCount.Value < 0) _inventoryCount.Value = 0;
    }
    public void Register(IItemEffectReceiver itemEffectReceiver)
    {
        _itemEffectReceivers.Add(itemEffectReceiver);
    }
    public void Lift(IItemEffectReceiver itemEffectReceiver)
    {
        _itemEffectReceivers.Remove(itemEffectReceiver);
    }
    /// <summary>
    /// アイテムを使用する。所持数が0以下の場合無効。
    /// </summary>
    public void UseItem(IItemUser itemUser)
    {
        if (_inventoryCount.Value <= 0)
        {
            Debug.Log($"{_name} の所持数は {_inventoryCount.Value}です。");
            return;
        }
        foreach (var e in _itemEffectReceivers)
        {
            if (_itemEffectFilter.CanUsedItem(itemUser, e))
                e.ReceiveItemEffect(this, itemUser);
        }
        // 所持数を減らす。
        _inventoryCount.Value--;
    }
}