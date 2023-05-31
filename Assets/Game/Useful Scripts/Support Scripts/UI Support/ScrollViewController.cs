// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// アイテムUIのスクロールを制御するコンポーネント
/// </summary>
public class ItemScrollViewController : MonoBehaviour
{
    [SerializeField]
    private float _lineHeight;

    private RectTransform _scrollViewRectTransform = null;
    private bool _isFirstCall = true;//最初は調整しない

    private void Awake()
    {
        _scrollViewRectTransform = GetComponent<RectTransform>();
    }

    void Update_ScrollPos(GameObject oldSelected, GameObject newSelected)
    {
        //なぜか最初の一週目はでたらめな数字が入っているので調整しない
        if (_isFirstCall)
        {
            _isFirstCall = false;
            return;
        }
        if (EventSystem.current == null)
        {
            return;
        }
        var currentButtonRectTransform = newSelected.GetComponent<RectTransform>();

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            //上にはみ出していないか判定する
            if (_scrollViewRectTransform.anchoredPosition.y > //枠の上辺
                -currentButtonRectTransform.anchoredPosition.y) //ボタンの上辺(枠が正の値に大きくなるのに対して、ボタンは負の値が大きくなるので、片方だけ正負反転する)
            {
                //はみ出していれば位置を調整する
                _scrollViewRectTransform.anchoredPosition = Vector2.up * -currentButtonRectTransform.anchoredPosition;
            }

            //下にはみ出していないか判定する
            if (_scrollViewRectTransform.anchoredPosition.y + _scrollViewRectTransform.sizeDelta.y <
                -(currentButtonRectTransform.anchoredPosition.y - _lineHeight))//ボタンの下辺
            {
                //はみ出していれば位置を調整する
                _scrollViewRectTransform.anchoredPosition = Vector3.up * -(currentButtonRectTransform.anchoredPosition.y + _scrollViewRectTransform.sizeDelta.y - _lineHeight);
            }
        }
    }
}