// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class ItemManager
{
    private const int MaxItemID = 4;
    private static Item[] _itemData = null;
    private const string _itemDataCsvAddressableName = "Item Data Csv";

    private static bool _isSetuped = false;

    /// <summary> アイテムを使用する際、確認ウィンドウを表示するかどうか。 </summary>
    public static bool ShouldConfirmItemUsage { get; set; } = true;

    public async static UniTask<Item> GetItemData(int id, CancellationToken token)
    {
        if (_itemData == null) Setup();
        try
        {
            await UniTask.WaitUntil(IsSetuped, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        return _itemData[id];
    }
    public async static UniTask<Item[]> GetItemDataAll(CancellationToken token)
    {
        if (_itemData == null) Setup();
        try
        {
            await UniTask.WaitUntil(IsSetuped, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        return _itemData;
    }
    private static bool IsSetuped()
    {
        return _isSetuped;
    }

    private async static void Setup()
    {
        // アイテムの配列を確保する。
        _itemData = new Item[MaxItemID];
        // ここでcsvファイルから情報を読み込み、Itemインスタンスを確保する。
        // アドレッサブルから読み込む準備をする。
        var asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(_itemDataCsvAddressableName);
        // アドレッサブルから読み込む。
        await asyncOperationHandle.Task;
        // 読み込み用テキストを改行区切りで分割する。
        var allData = asyncOperationHandle.Result.text.Split('\n');
        // インデックス カウンタ変数を用意する。0行目はヘッダー行なので無視する。
        int index = 1;
        // 読み込み可能である限りループする。
        while (index <= MaxItemID && !string.IsNullOrEmpty(allData[index]))
        {
            // カンマ区切りで分割する。
            var data = allData[index].Split(',');
            // インデックスは1から始まるが,格納場所は0番目からの為 -1する。
            _itemData[index - 1] = new Item(index - 1, data[1], data[2],
                StringToEffectType(data[3]), StringToTargetType(data[4]), await GetItemEffectFilter(data[5]));

            index++;
        }
        // アドレッサブルを開放する。
        Addressables.Release(asyncOperationHandle);
        _isSetuped = true;
    }
    private static ItemEffectType StringToEffectType(string effectName)
    {
        switch (effectName)
        {
            case "Healing": return ItemEffectType.Healing;
            case "Attack": return ItemEffectType.Attack;
            case "Support": return ItemEffectType.Support;
            case "Valuables": return ItemEffectType.Valuables;
            default: Debug.LogError($"想定されてない値が渡されました。:{effectName}"); throw new InvalidCastException();
        }
    }
    private static ItemUseTargetType StringToTargetType(string targetTypeName)
    {
        switch (targetTypeName)
        {
            case "ToSelf": return ItemUseTargetType.ApplyEffectToSelf;
            case "ToTarget": return ItemUseTargetType.ApplyEffectToTarget;
            case "ToArea": return ItemUseTargetType.ApplyEffectToArea;
            case "ToAll": return ItemUseTargetType.ApplyEffectToAll;
            default: Debug.LogError($"想定されてない値が渡されました。:{targetTypeName}"); throw new InvalidCastException();
        }
    }
    /// <summary> 非同期でスクリプタブルオブジェクトを取得する。 </summary>
    /// <param name="addressableName"></param>
    /// <returns></returns>
    private static async UniTask<ItemEffectFilterBase> GetItemEffectFilter(string addressableName)
    {
        await UniTask.Yield(); // コンパイルエラー回避の為の仮置き。
        return null; // コンパイルエラー回避の為の仮置き。
    }
}
