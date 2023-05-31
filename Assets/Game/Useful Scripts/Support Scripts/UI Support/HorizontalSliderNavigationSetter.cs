// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace UISupport
{
    public class HorizontalSliderNavigationSetter : MonoBehaviour
    {
        [Header("上入力が発生したときの遷移先")]
        [SerializeField]
        private Selectable[] _upSelectables = default;
        [Header("下入力が発生したときの遷移先")]
        [SerializeField]
        private Selectable[] _downSelectables = default;

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
            navigation.selectOnUp = SearchInteractableButton(_upSelectables, _origin);
            // 下の設定
            navigation.selectOnDown = SearchInteractableButton(_downSelectables, _origin);
            // 右を設定
            navigation.selectOnRight = _origin;
            // 左を設定
            navigation.selectOnLeft = _origin;

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