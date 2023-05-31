// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UISupport
{
    public class VerticalSliderNavigationSetter : MonoBehaviour
    {
        [Header("右入力が発生したときの遷移先")]
        [SerializeField]
        private Selectable[] _rightSelectables = default;
        [Header("左入力が発生したときの遷移先")]
        [SerializeField]
        private Selectable[] _leftSelectables = default;

        [Header("決定ボタン押下時の遷移先")]
        [SerializeField]
        private Selectable[] _determinationButtons = default;

        [Header("決定ボタン押下時に遷移先の設定を更新するかどうか")]
        [SerializeField]
        private bool _isUpdateNavigationOnDecisionInputNeeded = false;

        private Slider _origin = null;

        private void Start()
        {
            _origin = GetComponent<Slider>();
            UpdateNavigation();
        }
        private void Update()
        {
            // 場合によっては一か所で入力を監視し、処理を一任する。
            // オブジェクトごとに入力を監視,判定するのは無駄なコスト。
            if (_isUpdateNavigationOnDecisionInputNeeded &&
                EventSystem.current.currentSelectedGameObject == _origin.gameObject &&
                (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
            {
                EventSystem.current.SetSelectedGameObject(SearchInteractableButton(_determinationButtons, _origin).gameObject);
                UpdateNavigation();
            }
        }

        private void UpdateNavigation()
        {
            // get the Navigation data
            Navigation navigation = _origin.navigation;
            navigation.mode = Navigation.Mode.Explicit;

            // 上の設定
            navigation.selectOnUp = _origin;
            // 下の設定
            navigation.selectOnDown = _origin;
            // 右を設定
            navigation.selectOnRight = SearchInteractableButton(_rightSelectables, _origin);
            // 左を設定
            navigation.selectOnLeft = SearchInteractableButton(_leftSelectables, _origin);

            _origin.navigation = navigation;
        }
        private Selectable SearchInteractableButton(Selectable[] buttons, Selectable origin)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].interactable)
                {
                    return buttons[i];
                }
            }
            return origin;
        }
    }
}