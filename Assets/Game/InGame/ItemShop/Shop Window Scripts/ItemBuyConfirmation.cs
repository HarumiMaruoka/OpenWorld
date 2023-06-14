// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class ItemBuyConfirmation : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private Text _currentStockText = default;
    [SerializeField]
    private Button _buyButton = default;
    [SerializeField]
    private Button _notBuyButton = default;

    private bool _isJudged = false;
    private bool _isBuy = false;
    private bool _isNotBuy = false;

    public Text Text => _text;
    public Text CurrentStockText => _currentStockText;
    public bool IsBuy => _isBuy;
    public bool IsNotBuy => _isNotBuy;

    private void Awake()
    {
        _buyButton.onClick.AddListener(Buy);
        _notBuyButton.onClick.AddListener(NotBuy);
    }
    public bool IsJudged()
    {
        return _isJudged;
    }
    private void Buy()
    {
        _isJudged = true;
        _isBuy = true;
    }
    private void NotBuy()
    {
        _isJudged = true;
        _isNotBuy = true;
    }
    public void Clear()
    {
        _isJudged = false;
        _isBuy = false;
        _isNotBuy = false;
    }
}
