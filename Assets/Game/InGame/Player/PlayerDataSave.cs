// 日本語対応
using UnityEngine;

public class PlayerDataSave : InGameDataSaveBase
{
    [SerializeField]
    PlayerLife _playerLife = default;
    [SerializeField]
    PlayerMoneyController _playerMoneyController = default;

    private void Awake()
    {
        OnSaveSetup += SaveSetup;
    }
    private void OnDestroy()
    {
        OnSaveSetup -= SaveSetup;
    }

    private void SaveSetup()
    {
        SaveLoadManager.SetSaveData(new SaveLoadManager.
            PlayerSaveData(transform.position, _playerLife.CurrentLife.Value, _playerMoneyController.Funds.Value));
    }
}
