// 日本語対応
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
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
        GameManager.Instance.SetGameStartMode(GameStartMode.Contienue);
        SceneManager.LoadScene(_nextSceneName);
    }
}
