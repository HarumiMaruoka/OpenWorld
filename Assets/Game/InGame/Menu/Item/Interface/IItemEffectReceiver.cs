// 日本語対応

using UnityEngine;
/// <summary>
/// アイテムの効果を受ける事ができる
/// オブジェクトが継承すべきインターフェース。
/// </summary>
public interface IItemEffectReceiver
{
    public void ReceiveItemEffect(Item item, IItemUser itemUser);
}
