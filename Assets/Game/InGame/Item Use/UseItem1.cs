// 日本語対応
using UnityEngine;

public class UseItem1 : ItemUseBase
{
    public override int MyID => 1;

    [SerializeField]
    private GameObject _effectObj;

    public override void ReceiveItemEffect()
    {
        //Debug.LogError("炎の奇石を使用した。");
        Instantiate(_effectObj, PlayerInfo.CurrentPlayerInfo.transform.position, Quaternion.identity);
    }
}
