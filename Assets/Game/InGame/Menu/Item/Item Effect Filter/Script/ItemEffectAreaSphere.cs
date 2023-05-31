// 日本語対応
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffectAreaSphere", menuName = "ItemEffectAreaSO/ItemEffectAreaSphere", order = 2)]
public class ItemEffectAreaSphere : ItemEffectFilterBase
{
    [Header("このチェックボックスが有効であれば、壁越しの相手は判定しない。")]
    [SerializeField]
    private bool _isDetermineWall = true;
    [Header("半径")]
    [SerializeField]
    private float _radius = 1f;

    public override bool CanUsedItem(IItemUser itemUser, IItemEffectReceiver receiver)
    {
        throw new System.NotImplementedException();
    }
}
