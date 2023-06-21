// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ItemShopExplanatoryTextController : MonoBehaviour
{
    [SerializeField]
    private Text _explanatoryText = default;

    private void OnEnable()
    {
        _explanatoryText.text = null;
        UIManager.Current.OnChangedSelectedObject += OnChangedSelectedObject;
    }
    private void OnDisable()
    {
        if (UIManager.Current == null) return;
        UIManager.Current.OnChangedSelectedObject -= OnChangedSelectedObject;
    }
    private async void OnChangedSelectedObject(GameObject _, GameObject newObj)
    {
        if (newObj == null) return;
        if (newObj.TryGetComponent(out ItemShopProductButton button))
        {
            _explanatoryText.text =
                (await ItemManager.GetItemData(button.Product.ItemID,
                 this.GetCancellationTokenOnDestroy())).ExplanatoryText;
        }
        else
        {
            _explanatoryText.text = null;
        }
    }
}
