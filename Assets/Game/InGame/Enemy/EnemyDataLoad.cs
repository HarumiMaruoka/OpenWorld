// 日本語対応
using System.Collections.Generic;
using UnityEngine;

// ジェネレーターとして活動する。
public class EnemyDataLoad : InGameDataRestorationBase
{
    [SerializeField]
    private GameObject _enemyPrefab = default;

    private readonly string _enemySaveDataFileName = "EnemySaveData";

    private static HashSet<EnemySaveDataSet> _enemySaveData = new HashSet<EnemySaveDataSet>();

    private EnemySaveDataSet _saveDataSet;

    // エネミーデータの復元

    private void Awake()
    {
        OnLoad += Load;
    }

    private void Load()
    {
        var enemyData = SaveLoadManager.Load<HashSet<EnemySaveDataSet>>(_enemySaveDataFileName);
        foreach (var e in enemyData)
        {
            Instantiate(_enemyPrefab, e.Position.position, Quaternion.identity);
        }
    }
}
