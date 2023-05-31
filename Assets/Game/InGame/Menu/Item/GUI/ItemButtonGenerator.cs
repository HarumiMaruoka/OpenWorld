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

    private async void Awake()
    {
        Generate(await ItemManager.GetItemData(this.gameObject.GetCancellationTokenOnDestroy()));
    }

    private void Generate(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var button = Instantiate(_itemButtonPrefab, _allParent);
            button.Setup(i);

            ItemButton itemButton;
            switch (items[i].EffectType)
            {
                case ItemEffectType.Healing:
                    itemButton = Instantiate(_itemButtonPrefab, _healingParent);
                    itemButton.Setup(i);
                    break;
                case ItemEffectType.Attack:
                    itemButton = Instantiate(_itemButtonPrefab, _attackParent);
                    itemButton.Setup(i);
                    break;
                case ItemEffectType.Support:
                    itemButton = Instantiate(_itemButtonPrefab, _supportParent);
                    itemButton.Setup(i);
                    break;
                case ItemEffectType.Valuables:
                    itemButton = Instantiate(_itemButtonPrefab, _valuablesParent);
                    itemButton.Setup(i);
                    break;
            }
        }
    }
}
