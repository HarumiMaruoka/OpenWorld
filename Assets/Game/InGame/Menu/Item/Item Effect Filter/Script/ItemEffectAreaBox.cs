// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffectAreaBox", menuName = "ItemEffectAreaSO/ItemEffectAreaBox", order = 1)]
public class ItemEffectAreaBox : ItemEffectFilterBase
{
    [SerializeField]
    private Vector3 _halfExtents = Vector3.one;

    public override bool CanUsedItem(IItemUser itemUser, IItemEffectReceiver receiver)
    {
        throw new System.NotImplementedException();
    }
}
