// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class EnemyDataBase
{
    private const int _maxEnemyID = 1;
    private const string _enemyStatusCsvAddressableName = "Enemy Status Data Csv";

    private static EnemyStatus[] _enemyStatusData = null;

    private static bool _isSetuped = false;

    private static bool IsSetuped()
    {
        return _isSetuped;
    }

    public static async UniTask<EnemyStatus> GetEnemyStatus(int id, CancellationToken token)
    {
        if (_enemyStatusData == null) Setup();
        try
        {
            await UniTask.WaitUntil(IsSetuped, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        return _enemyStatusData[id];
    }
    private static async void Setup()
    {
        // エネミーステータスの配列を確保する。
        _enemyStatusData = new EnemyStatus[_maxEnemyID];
        // アドレッサブルから読み込む準備をする。
        var asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(_enemyStatusCsvAddressableName);
        // アドレッサブルから読み込む。
        await asyncOperationHandle.Task;
        // 読み込み用テキストを改行区切りで分割する。
        var allData = asyncOperationHandle.Result.text.Split('\n');
        // インデックス カウンタ変数を用意する。0行目はヘッダー行なので無視する。
        int index = 1;
        // 読み込み可能である限りループする。
        while (index <= _maxEnemyID && !string.IsNullOrEmpty(allData[index]))
        {
            // カンマ区切りで分割する。
            var data = allData[index].Split(',');
            // インデックスは1から始まるが,格納場所は0番目からの為 -1する。
            _enemyStatusData[index - 1] = new EnemyStatus(
                 data[0], float.Parse(data[1]));

            index++;
        }
        // アドレッサブルを開放する。
        Addressables.Release(asyncOperationHandle);
        _isSetuped = true;
    }
}
public struct EnemyStatus
{
    public EnemyStatus(string name = "", float life = 1f)
    {
        _name = name; _life = life;
    }

    private string _name;
    private float _life;

    public string Name { get => _name; }
    public float MaxLife { get => _life; }
}