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

    private void Load()
    {
        // プレイヤーの座標を復元する。
        transform.position = SaveLoadManager.Load<Vector3>(_playerPosFileName);
        // プレイヤーのライフを復元する。
        _playerLife.SetLife(SaveLoadManager.Load<float>(_playerLifeFileName));
        // プレイヤーの所持金を復元する。
        _playerMoneyController.SetFunds(SaveLoadManager.Load<long>(_playerFundsFileName));
    }
}
