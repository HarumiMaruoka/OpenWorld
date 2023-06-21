// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ItemDataSave : InGameDataSaveBase
{
    private void Awake()
    {
        OnSaveSetup += SaveSetup;
    }
    private void OnDestroy()
    {
        OnSaveSetup -= SaveSetup;
    }

    private async void SaveSetup()
    {
        var items = await ItemManager.GetItemDataAll(this.GetCancellationTokenOnDestroy());

        var saveData = new int[items.Length];
        for (int i = 0; i < saveData.Length; i++)
        {
            saveData[i] = items[i].InventoryCount.Value;
        }

        SaveLoadManager.SetSaveData(new SaveLoadManager.ItemSaveData(saveData)); ;
    }
}
