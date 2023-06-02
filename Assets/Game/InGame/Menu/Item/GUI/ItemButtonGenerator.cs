// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemButtonGenerator : MonoBehaviour
{
    [Header("生成するアイテムボタンのプレハブ")]
    [SerializeField]
    private ItemButton _itemButtonPrefab = default;

    [Header("全アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _allParent = default;
    [Header("回復アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _healingParent = default;
    [Header("攻撃アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _attackParent = default;
    [Header("サポートアイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _supportParent = default;
    [Header("貴重品アイテムボタンの親オブジェクト")]
    [SerializeField]
    private Transform _valuablesParent = default;

    [Header("以下 アイテムボタンに渡す値")]
    [Header("アイテムを使用する際の確認ウィンドウ")]
    [SerializeField]
    private ItemIsUsedWindow _checkItemIsUsedWindow = default;

    private async void Awake()
    {
        Generate(await ItemManager.GetItemDataAll(this.gameObject.GetCancellationTokenOnDestroy()));
    }

    private void Generate(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var button = Instantiate(_itemButtonPrefab, _allParent);
            button.Setup(i);
            button.SetCheckItemIsUsedWindow(_checkItemIsUsedWindow);

            ItemButton itemButton;
            switch (items[i].EffectType)
            {
                case ItemEffectType.Healing:
                    itemButton = Instantiate(_itemButtonPrefab, _healingParent);
                    itemButton.Setup(i);
                    itemButton.SetCheckItemIsUsedWindow(_checkItemIsUsedWindow);
                    break;
                case ItemEffectType.Attack:
                    itemButton = Instantiate(_itemButtonPrefab, _attackParent);
                    itemButton.Setup(i);
                    itemButton.SetCheckItemIsUsedWindow(_checkItemIsUsedWindow);
                    break;
                case ItemEffectType.Support:
                    itemButton = Instantiate(_itemButtonPrefab, _supportParent);
                    itemButton.Setup(i);
                    itemButton.SetCheckItemIsUsedWindow(_checkItemIsUsedWindow);
                    break;
                case ItemEffectType.Valuables:
                    itemButton = Instantiate(_itemButtonPrefab, _valuablesParent);
                    itemButton.Setup(i);
                    itemButton.SetCheckItemIsUsedWindow(_checkItemIsUsedWindow);
                    break;
            }
        }
    }
}
