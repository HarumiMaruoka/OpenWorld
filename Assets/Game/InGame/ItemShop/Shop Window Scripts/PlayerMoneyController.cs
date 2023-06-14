// 日本語対応
using UnityEngine;
using UniRx;

public class PlayerMoneyController : MonoBehaviour
{
    private static PlayerMoneyController _current = null;
    public static PlayerMoneyController Current => _current;

    private const long MaxFunds = 1000000;

    [SerializeField]
    private long _initialFunds = 10000;

    private LongReactiveProperty _funds = new LongReactiveProperty(0);
    public IReadOnlyReactiveProperty<long> Funds => _funds;

    private void Awake()
    {
        _funds.Value = _initialFunds;
        _current = this;
    }
    private void OnDestroy()
    {
        _current = null;
    }
    public void SetFunds(long value)
    {
        _funds.Value = value;
    }

    public void EarnMoney(long value)
    {
        _funds.Value = _funds.Value + value;
        if (_funds.Value > MaxFunds)
        {
            _funds.Value = MaxFunds;
        }
    }
    public bool TryDeductMoney(long value)
    {
        if (_funds.Value - value > 0)
        {
            _funds.Value = _funds.Value - value;
            return true;
        }
        Debug.LogError("想定以上のお金が支払われようとしました！");
        return false;
    }
}
