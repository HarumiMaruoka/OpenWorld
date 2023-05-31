// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFilterButtonController : MonoBehaviour
{
    [Header("アイテムのスクロールビュー")]
    [SerializeField]
    private ScrollRect _scrollRect = default;

    [Header("全アイテムボタンの親オブジェクト")]
    [SerializeField]
    private RectTransform _allParent = default;
    [Header("回復アイテムボタンの親オブジェクト")]
    [SerializeField]
    private RectTransform _healingParent = default;
    [Header("攻撃アイテムボタンの親オブジェクト")]
    [SerializeField]
    private RectTransform _attackParent = default;
    [Header("サポートアイテムボタンの親オブジェクト")]
    [SerializeField]
    private RectTransform _supportParent = default;
    [Header("貴重品アイテムボタンの親オブジェクト")]
    [SerializeField]
    private RectTransform _valuablesParent = default;

    private Dictionary<ItemButtonType, RectTransform> _buttonParents = new Dictionary<ItemButtonType, RectTransform>();
    private ItemButtonType _currentItemButtonType = ItemButtonType.All;

    /// <summary> アイテムのフィルターボタンが押された時に実行する。 </summary>
    public event Action<ItemButtonType, ItemButtonType> OnChangedFillterType = default;

    private void Awake()
    {
        _buttonParents.Add(ItemButtonType.All, _allParent);
        _buttonParents.Add(ItemButtonType.Healing, _healingParent);
        _buttonParents.Add(ItemButtonType.Attack, _attackParent);
        _buttonParents.Add(ItemButtonType.Support, _supportParent);
        _buttonParents.Add(ItemButtonType.Valuables, _valuablesParent);

        foreach (var e in _buttonParents)
        {
            e.Value.gameObject.SetActive(false);
        }

        _buttonParents[_currentItemButtonType].gameObject.SetActive(true);
        _scrollRect.content = _buttonParents[_currentItemButtonType];
    }

    public void ChangeFilterType(ItemButtonType value)
    {
        // 同じであれば処理しない。
        if (_currentItemButtonType == value) return;
        // 必要なオブジェクトを有効にし、不必要なオブジェクトを無効にする。
        _buttonParents[_currentItemButtonType].gameObject.SetActive(false);
        OnChangedFillterType?.Invoke(_currentItemButtonType, value);
        _currentItemButtonType = value;
        _buttonParents[_currentItemButtonType].gameObject.SetActive(true);
        // コンテントを入れ替える。
        _scrollRect.content = _buttonParents[_currentItemButtonType];
    }
}
[Serializable]
public enum ItemButtonType
{
    All, Healing, Attack, Support, Valuables
}
