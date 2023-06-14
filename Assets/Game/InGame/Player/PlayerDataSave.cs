// 日本語対応
using UnityEngine;

public class PlayerDataSave : InGameDataSaveBase
{
    [SerializeField]
    PlayerLife _playerLife = default;
    [SerializeField]
    PlayerMoneyController _playerMoneyController = default;

    private readonly string _playerPosFileName = "PlayerPosition";
    private readonly string _playerLifeFileName = "PlayerLife";
    private readonly string _playerFundsFileName = "PlayerFunds";

    private void Awake()
    {
        OnSave += Save;
    }

    private void Save()
    {
        // プレイヤーの座標を保存する。
        SaveLoadManager.Save(transform.position, _playerPosFileName);
        // プレイヤーのライフを保存する。
        SaveLoadManager.Save(_playerLife.CurrentLife.Value, _playerLifeFileName);
        // プレイヤーの所持金を保存する。
        SaveLoadManager.Save(_playerMoneyController.Funds.Value, _playerFundsFileName);
    }
}
