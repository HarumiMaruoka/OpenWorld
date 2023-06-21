// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemDataLoad : InGameDataRestorationBase
{
    private void Awake()
    {
        OnLoad += Load;
    }

    private async void Load(SaveLoadManager.SaveDataSet saveData)
    {
        var items = await ItemManager.GetItemDataAll(this.GetCancellationTokenOnDestroy());
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetInventoryCount(saveData.ItemSaveData.InventoryCount[i]);
        }
    }
}
