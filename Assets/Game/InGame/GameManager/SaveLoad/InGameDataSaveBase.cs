
// 日本語対応
using System;
using UnityEngine;

public class InGameDataSaveBase : MonoBehaviour
{
    protected event Action OnSaveSetup = default;

    protected virtual void OnEnable()
    {
        SaveLoadManager.OnSaveSetup += OnSaveSetup;
    }
    protected virtual void OnDisable()
    {
        SaveLoadManager.OnSaveSetup -= OnSaveSetup;
    }
}