// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class ItemUseBase : MonoBehaviour, IItemEffectReceiver
{
    public abstract int MyID { get; }

    private async void Awake()
    {
        var item = await ItemManager.GetItemData(MyID, this.GetCancellationTokenOnDestroy());
        item.Register(this);
    }

    public abstract void ReceiveItemEffect();
}
