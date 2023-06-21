// 日本語対応
using UnityEngine;

public class UseItem2 : ItemUseBase
{
    public override int MyID => 2;

    public override void ReceiveItemEffect()
    {
        Debug.LogError("プロテインを使用した。");
    }
}
