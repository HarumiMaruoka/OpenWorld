// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class TalkCanvasController : MonoBehaviour
{
    [SerializeField]
    private Text _nameText = default;
    [SerializeField]
    private Text _talkText = default;

    public Text NameText => _nameText;
    public Text TalkText => _talkText;
}
