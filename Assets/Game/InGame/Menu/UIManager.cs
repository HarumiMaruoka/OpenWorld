// 日本語対応
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static UIManager _current = default;
    public static UIManager Current => _current;

    private void Awake()
    {
#if UNITY_EDITOR
        if (_current != null)
        {
            Debug.LogError("UIManagerが複数個あるかもよ。あるいは開放し忘れてるかもよ。");
        }
#endif
        _current = this;
    }
    private void OnDestroy()
    {
        _current = null;
    }

    private GameObject _previousSelectedObject = null;
    /// <summary>
    /// EventSystemで選択中のオブジェクトが変更されたときに発行するイベント。
    /// 第一引数に変更前のオブジェクト、第二引数に変更後のオブジェクトが渡される。
    /// </summary>
    public event Action<GameObject, GameObject> OnChangedSelectedObject = default;

    private void Update()
    {
        if (EventSystem.current == null) return;

        if (EventSystem.current.currentSelectedGameObject != _previousSelectedObject)
        {
            OnChangedSelectedObject?.Invoke(_previousSelectedObject, EventSystem.current.currentSelectedGameObject);
        }
        _previousSelectedObject = EventSystem.current.currentSelectedGameObject;
    }
}
