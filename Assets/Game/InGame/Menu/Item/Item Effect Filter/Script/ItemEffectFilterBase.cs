// 日本語対応
using UnityEngine;

/// <summary>
/// アイテムの使用効果を指定する用のスクリプタブルオブジェクト。
/// 継承して、箱状や、球状に判定を取るようにする事が目的。
/// </summary>
public abstract class ItemEffectFilterBase : ScriptableObject
{
    public abstract bool CanUsedItem(IItemUser itemUser, IItemEffectReceiver receiver);
}
