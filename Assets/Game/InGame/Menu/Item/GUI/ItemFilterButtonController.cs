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


    [Header("AllフィルターボタンのImage")]
    [SerializeField]
    private Image _allButtonImage = default;
    [Header("回復フィルターボタンのImage")]
    [SerializeField]
    private Image _healingButtonImage = default;
    [Header("攻撃フィルターボタンのImage")]
    [SerializeField]
    private Image _attackButtonImage = default;
    [Header("サポートフィルターボタンのImage")]
    [SerializeField]
    private Image _supportButtonImage = default;
    [Header("貴重品フィルターボタンのImage")]
    [SerializeField]
    private Image _valuablesButtonImage = default;

    [Header("非選択中スプライト")]
    [SerializeField]
    private Sprite _nomalSprite = default;
    [Header("選択中スプライト")]
    [SerializeField]
    private Sprite _selectedSprite = default;

    private Dictionary<ItemButtonType, RectTransform> _buttonParents = new Dictionary<ItemButtonType, RectTransform>();
    private Dictionary<ItemButtonType, Image> _fillterButtonImages = new Dictionary<ItemButtonType, Image>();
    private ItemButtonType _currentItemButtonType = ItemButtonType.All;

    /// <summary> アイテムのフィルターボタンが押された時に実行する。 </summary>
    public event Action<ItemButtonType, ItemButtonType> OnChangedFillterType = default;


    public bool _isSetuped = false;
    public bool IsSetuped() { return _isSetuped; }


    private void Awake()
    {
        _buttonParents.Add(ItemButtonType.All, _allParent);
        _buttonParents.Add(ItemButtonType.Healing, _healingParent);
        _buttonParents.Add(ItemButtonType.Attack, _attackParent);
        _buttonParents.Add(ItemButtonType.Support, _supportParent);
        _buttonParents.Add(ItemButtonType.Valuables, _valuablesParent);

        _fillterButtonImages.Add(ItemButtonType.All, _allButtonImage);
        _fillterButtonImages.Add(ItemButtonType.Healing, _healingButtonImage);
        _fillterButtonImages.Add(ItemButtonType.Attack, _attackButtonImage);
        _fillterButtonImages.Add(ItemButtonType.Support, _supportButtonImage);
        _fillterButtonImages.Add(ItemButtonType.Valuables, _valuablesButtonImage);

        foreach (var e in _buttonParents)
        {
            e.Value.gameObject.SetActive(false);
        }
        foreach (var e in _fillterButtonImages)
        {
            e.Value.sprite = _nomalSprite;
        }

        _fillterButtonImages[_currentItemButtonType].sprite = _selectedSprite;
        _buttonParents[_currentItemButtonType].gameObject.SetActive(true);
        _scrollRect.content = _buttonParents[_currentItemButtonType];

        _isSetuped = true;
    }

    public void ChangeFilterType(ItemButtonType value)
    {
        // 同じであれば処理しない。
        if (_currentItemButtonType == value) return;
        // 必要なオブジェクトを有効にし、不必要なオブジェクトを無効にする。
        _buttonParents[_currentItemButtonType].gameObject.SetActive(false);
        _fillterButtonImages[_currentItemButtonType].sprite = _nomalSprite;
        OnChangedFillterType?.Invoke(_currentItemButtonType, value);
        _currentItemButtonType = value;
        _buttonParents[_currentItemButtonType].gameObject.SetActive(true);
        _fillterButtonImages[_currentItemButtonType].sprite = _selectedSprite;
        // コンテントを入れ替える。
        _scrollRect.content = _buttonParents[_currentItemButtonType];
    }
}
[Serializable]
public enum ItemButtonType
{
    All, Healing, Attack, Support, Valuables
}
