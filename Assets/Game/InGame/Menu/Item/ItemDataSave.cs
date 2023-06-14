// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ItemDataSave : InGameDataSaveBase
{
    private void Awake()
    {
        OnSave += Save;
    }

    private async void Save()
    {
        var items = await ItemManager.GetItemDataAll(this.GetCancellationTokenOnDestroy());
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SaveInventoryCount();
        }
    }
}
