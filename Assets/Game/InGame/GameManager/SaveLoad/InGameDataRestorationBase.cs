// 日本語対応
using System;
using UnityEngine;

public class InGameDataRestorationBase : MonoBehaviour
{
    protected event Action OnLoad = default;

    protected virtual void OnEnable()
    {
        SaveLoadManager.OnInGameDataLoad += OnLoad;
    }
    protected virtual void OnDisable()
    {
        SaveLoadManager.OnInGameDataLoad -= OnLoad;
    }
}
