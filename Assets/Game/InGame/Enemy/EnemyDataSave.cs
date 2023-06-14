// 日本語対応
using System.Collections.Generic;
using UnityEngine;

// エネミー自身にアタッチする
public class EnemyDataSave : InGameDataSaveBase
{
    private readonly string _enemySaveDataFileName = "EnemySaveData";

    private static HashSet<EnemySaveDataSet> _enemySaveData = new HashSet<EnemySaveDataSet>();

    private EnemySaveDataSet _saveDataSet;

    private void Awake()
    {
        OnSave += Save;
        _enemySaveData.Add(_saveDataSet);
        _saveDataSet = new EnemySaveDataSet(transform, GetComponent<SampleEnemyLife>());
    }
    private void OnDestroy()
    {
        _enemySaveData.Remove(_saveDataSet);
    }

    private void Save()
    {
        SaveLoadManager.Save(_enemySaveData, _enemySaveDataFileName);
    }
}

[System.Serializable]
public struct EnemySaveDataSet
{
    public EnemySaveDataSet(Transform pos, SampleEnemyLife life)
    {
        _position = pos; _life = life;
    }

    [SerializeField]
    private Transform _position;
    [SerializeField]
    private SampleEnemyLife _life;

    public Transform Position => _position;
    public SampleEnemyLife Life => _life;
}