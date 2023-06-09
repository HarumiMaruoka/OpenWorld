// 日本語対応
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class EnemyReactionUI : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;
    [SerializeField]
    private Ease _ease = default;

    [SerializeField]
    private float _openAnimDuration = 1f;
    [SerializeField]
    private float _closeAnimDuration = 1f;

    private TweenerCore<Vector3, Vector3, VectorOptions> _currentAnim = null;

    public bool IsOpened { get; private set; } = false;
    public Status CurrentStatus { get; private set; } = Status.None;

    private void Awake()
    {
        _image.rectTransform.localScale = Vector3.zero;
    }

    public virtual void Open()
    {
        if (_currentAnim != null) _currentAnim.Kill();
        _image.enabled = true;
        IsOpened = true;
        CurrentStatus = Status.Opening;
        _currentAnim = _image.rectTransform.DOScale(Vector3.one, _openAnimDuration).SetEase(_ease).
            OnComplete(OnOpenAnimEnd);
    }

    public virtual void Close()
    {
        if (_currentAnim != null) _currentAnim.Kill();
        CurrentStatus = Status.Closing;
        _currentAnim = _image.rectTransform.DOScale(Vector3.zero, _closeAnimDuration).SetEase(_ease).
               OnComplete(OnCloseAnimEnd);
    }

    private void OnOpenAnimEnd()
    {
        _currentAnim = null;
        CurrentStatus = Status.None;
    }
    private void OnCloseAnimEnd()
    {
        _currentAnim = null;
        _image.enabled = false;
        IsOpened = false;
        CurrentStatus = Status.None;
    }

    public enum Status
    {
        None, Opening, Closing
    }
}

