// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// アイテムUIのスクロールを制御するコンポーネント
/// </summary>
public class ScrollViewAutoScroller : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager = default;
    [SerializeField]
    private float _cellHeight = 100f;
    [SerializeField]
    private float _viewHeight = 472f;

    private RectTransform _scrollViewRectTransform = null;
    private VerticalLayoutGroup _verticalLayoutGroup = null;

    private void Awake()
    {
        _scrollViewRectTransform = GetComponent<RectTransform>();
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        UIManager.Current.OnChangedSelectedObject += UpdateScrollPos;
    }

    void UpdateScrollPos(GameObject oldSelected, GameObject newSelected)
    {
        if (newSelected == null) return;
        if (newSelected.GetComponent<ItemButton>() == null) return;
        var currentButtonRectTransform = newSelected.GetComponent<RectTransform>();

        ////上にはみ出していないか判定する
        if (_scrollViewRectTransform.anchoredPosition.y >= //枠の上辺
            -currentButtonRectTransform.anchoredPosition.y - _cellHeight / 2f - _verticalLayoutGroup.padding.top) //ボタンの上辺
        {
            //はみ出していれば位置を調整する
            _scrollViewRectTransform.anchoredPosition =
                new Vector2(_scrollViewRectTransform.anchoredPosition.x,
                -currentButtonRectTransform.anchoredPosition.y - _cellHeight / 2f - _verticalLayoutGroup.padding.top);
        }

        // 下にはみ出していないか判定する
        if (GetComponent<RectTransform>().anchoredPosition.y + _viewHeight <= //枠の下辺(500はScrollViewのHeight。シリアライズで取ってくるよう変更する)
            -(currentButtonRectTransform.anchoredPosition.y - _cellHeight / 2f))//ボタンの下辺
        {
            //はみ出していれば位置を調整する
            GetComponent<RectTransform>().anchoredPosition =
                Vector3.up * -(currentButtonRectTransform.anchoredPosition.y + _viewHeight -
                _cellHeight / 2f + _verticalLayoutGroup.padding.bottom + _verticalLayoutGroup.spacing);
        }
    }
}