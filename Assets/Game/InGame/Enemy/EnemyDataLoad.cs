// 日本語対応
using UnityEngine;

// ジェネレーターとして活動する。
public class EnemyDataLoad : InGameDataRestorationBase
{
    [SerializeField]
    private GameObject _enemyPrefab = default;

    private void Awake()
    {
        OnLoad += Load;
    }
    private void OnDestroy()
    {
        OnLoad -= Load;
    }

    private void Load(SaveLoadManager.SaveDataSet saveData)
    {
        // エネミーデータの復元
        var enemyData = saveData.EnemySaveData.EnemySaveDatas;
        foreach (var e in enemyData)
        {
            Instantiate(_enemyPrefab, e.Position, Quaternion.identity);
        }
    }
}
