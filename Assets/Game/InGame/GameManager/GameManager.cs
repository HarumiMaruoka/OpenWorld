// 日本語対応
using System;

public class GameManager : IDisposable
{
    private static GameManager _instance = new GameManager();
    public static GameManager Instance => _instance;
    private GameManager() { }

    private DeviceManager _deviceManager = new DeviceManager();

    public DeviceManager DeviceManager => _deviceManager;

    public void Dispose()
    {
        _deviceManager = null;
        GC.Collect();
    }

    private GameStartMode _gameStartMode = GameStartMode.NewGame;
    public GameStartMode GameStartMode => _gameStartMode;
    public void SetGameStartMode(GameStartMode gameStartMode)
    {
        _gameStartMode = gameStartMode;
    }
}
