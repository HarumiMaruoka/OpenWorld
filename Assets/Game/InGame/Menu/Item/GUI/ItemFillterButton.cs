// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class ItemFillterButton : MonoBehaviour
{
    [SerializeField]
    private ItemFilterButtonController _itemButtonController = default;
    [SerializeField]
    private ItemButtonType _itemButtonType = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }
    private void OnClicked()
    {
        _itemButtonController.ChangeFilterType(_itemButtonType);
    }
}
