// 日本語対応
using UnityEngine;

public class UseItem0 : ItemUseBase
{
    public override int MyID => 0;

    [SerializeField]
    private PlayerLife _playerLife = default;

    public override void ReceiveItemEffect()
    {
        // Debug.LogError("ヒールポーションを使用した。");
        // プレイヤーの体力を20%回復する。
        _playerLife.Heal(_playerLife.MaxLife / (100f / 20f));
    }
}
