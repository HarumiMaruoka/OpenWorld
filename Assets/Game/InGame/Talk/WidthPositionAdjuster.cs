// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 幅に合わせてx座標を調整するコンポーネント
public class WidthPositionAdjuster : UIBehaviour
{
    [SerializeField]
    private float _startingXPoint = default;
    [SerializeField]
    private Image _flameImage = default;
    [SerializeField]
    private Image _bgImage = default;

    private RectTransform _rectTransform = null;

    protected override void OnRectTransformDimensionsChange()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();

        var newPos = _rectTransform.anchoredPosition;
        newPos.x = _startingXPoint + _rectTransform.sizeDelta.x / 2f;
        _rectTransform.anchoredPosition = newPos;

        _bgImage.rectTransform.anchoredPosition = _rectTransform.anchoredPosition;

        var sizeDelta = _rectTransform.sizeDelta;

        var x = _flameImage.rectTransform.sizeDelta.x;
        var y = _flameImage.rectTransform.sizeDelta.y;

        sizeDelta.x += x;
        sizeDelta.y += y;

        _bgImage.rectTransform.sizeDelta = sizeDelta;
    }
}
