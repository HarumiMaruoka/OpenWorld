// 日本語対応
using UnityEngine;

public class InGameLoad : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.GameStartMode == GameStartMode.Contienue)
            SaveLoadManager.ExecuteInGameDataLoad();
    }
}
