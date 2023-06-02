// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonController : MonoBehaviour
{
    [SerializeField]
    private Text _itemExplanatoryText = default;

    private void OnEnable()
    {
        UIManager.Current.OnChangedSelectedObject += OnChangedSelectedObject;
    }
    private void OnDisable()
    {
        if (UIManager.Current != null)
            UIManager.Current.OnChangedSelectedObject -= OnChangedSelectedObject;
    }
    /// <summary> UIで選択しているオブジェクトが変更されたら、説明文を更新する。 </summary>
    /// <param name="oldObj"> 変更前のオブジェクト </param>
    /// <param name="newObj"> 変更後のオブジェクト </param>
    private async void OnChangedSelectedObject(GameObject oldObj, GameObject newObj)
    {
        if (newObj.TryGetComponent(out ItemButton itemButton))
        {
            _itemExplanatoryText.text = (await ItemManager.GetItemData(itemButton.MyItemID, this.GetCancellationTokenOnDestroy())).ExplanatoryText;
        }
        else
        {
            _itemExplanatoryText.text = null;
        }
    }
}
