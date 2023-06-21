// 日本語対応
using UnityEngine;

public class PlayerDataLoad : InGameDataRestorationBase
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
        OnLoad += Load;
    }

    private void Load(SaveLoadManager.SaveDataSet saveData)
    {
        // プレイヤーの座標を復元する。
        transform.position = saveData.PlayerSaveData.Position;
        // プレイヤーのライフを復元する。
        _playerLife.SetLife(saveData.PlayerSaveData.Life);
        // プレイヤーの所持金を復元する。
        _playerMoneyController.SetFunds(saveData.PlayerSaveData.Funds);
    }
}
