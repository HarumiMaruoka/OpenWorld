// 日本語対応
using System.Collections.Generic;
using UnityEngine;

// エネミー自身にアタッチする
public class EnemyDataSave : InGameDataSaveBase
{

    private readonly static SaveLoadManager.EnemySaveDataList _enemySaveDataList = new SaveLoadManager.EnemySaveDataList();

    private SampleEnemyLife _sampleEnemyLife = null;

    private void Awake()
    {
        OnSaveSetup += SaveSetup;
        SaveLoadManager.OnSaveEnd += Saved;
        _sampleEnemyLife = GetComponent<SampleEnemyLife>();
        SaveLoadManager.SetSaveData(new SaveLoadManager.EnemySaveDataList());
    }
    private void OnDestroy()
    {
        OnSaveSetup -= SaveSetup;
        SaveLoadManager.OnSaveEnd -= Saved;
    }

    private void SaveSetup()
    {
        _enemySaveDataList.EnemySaveDatas.Add(new SaveLoadManager.EnemySaveData(0, transform.position, _sampleEnemyLife.CurrentLife));
    }
    private void Saved()
    {
        _enemySaveDataList.EnemySaveDatas.Clear();
    }
}