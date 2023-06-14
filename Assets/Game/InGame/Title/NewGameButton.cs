// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    [SceneName, SerializeField]
    private string _nextSceneName = default;
    [SerializeField]
    private Button _button = default;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        GameManager.Instance.SetGameStartMode(GameStartMode.NewGame);
        SceneManager.LoadScene(_nextSceneName);
    }
}
