// 日本語対応
using UniRx;

public static class TimeStopManager
{
    private static BoolReactiveProperty _isTimeStop = new BoolReactiveProperty(false);
    public static IReadOnlyReactiveProperty<bool> IsTimeStop => _isTimeStop;

    public static void PauseTime()
    {
        _isTimeStop.Value = true;
    }
    public static void ResumeTime()
    {
        _isTimeStop.Value = false;
    }
}
