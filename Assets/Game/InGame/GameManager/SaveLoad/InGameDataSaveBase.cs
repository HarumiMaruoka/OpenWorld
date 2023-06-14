
// 日本語対応
using System;
using UnityEngine;

public class InGameDataSaveBase : MonoBehaviour
{
    protected event Action OnSave = default;

    protected virtual void OnEnable()
    {
        SaveLoadManager.OnInGameDataSave += OnSave;
    }
    protected virtual void OnDisable()
    {
        SaveLoadManager.OnInGameDataSave -= OnSave;
    }
}