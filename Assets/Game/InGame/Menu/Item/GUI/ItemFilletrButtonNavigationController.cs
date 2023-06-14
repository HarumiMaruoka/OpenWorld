// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class ItemFilletrButtonNavigationController : MonoBehaviour
{
    [SerializeField]
    private Selectable up = default;
    [SerializeField]
    private Selectable down = default;

    private Button _button = null;

    private void Awake()
    {
        _button = GetComponent<Button>();
        UIManager.Current.OnChangedSelectedObject += OnChangedSelectedObject;

        var navigation = _button.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        // 上はインスペクタで設定したオブジェクト
        navigation.selectOnUp = up;
        // 下もインスペクタで設定したオブジェクト
        navigation.selectOnDown = down;
        // 左は自身
        navigation.selectOnLeft = _button;

        _button.navigation = navigation;

    }
    private void OnChangedSelectedObject(GameObject oldObj, GameObject newObj)
    {
        // 右はさっきまで選択されてたやつ
        if (newObj == null) return;
        if (newObj.TryGetComponent(out ItemButton button))
        {
            var navigation = _button.navigation;
            navigation.selectOnRight = button.Button;
            _button.navigation = navigation;
        }
    }
}
