// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ItemIsUsedWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject _windowParent = default;

    [SerializeField]
    private Text _messageText = default;
    [SerializeField]
    private Button _allowButton = default;
    [SerializeField]
    private Button _disallowButton = default;

    private void Awake()
    {
        _allowButton.onClick.AddListener(AllowItemUsage);
        _disallowButton.onClick.AddListener(DisallowItemUsage);
    }

    public void OpenWindow()
    {
        _windowParent.SetActive(true);
    }
    public void CloseWindow()
    {
        _windowParent.SetActive(false);
        ClearMessageText();
    }
    public void SetMessageText(string itemName)
    {
        _messageText.text = $"{itemName} を使用しますか？";
    }
    public void ClearMessageText()
    {
        _messageText.text = null;
    }
    /// <summary> 決定を待つ </summary>
    public async UniTask<bool> WaitDecision(CancellationToken token)
    {
        try
        {
            await UniTask.WaitUntil(IsJudged, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        _isJudged = false;
        CloseWindow();
        return _isAllow;
    }

    private bool _isJudged = false;
    private bool IsJudged()
    {
        return _isJudged;
    }

    private bool _isAllow = false;
    public void AllowItemUsage()
    {
        // ボタンを押した事を表現する
        _isJudged = true;
        // アイテムを使う事にした
        _isAllow = true;
    }
    public void DisallowItemUsage()
    {
        // ボタンを押した事を表現する
        _isJudged = true;
        // アイテムを使わない事にした
        _isAllow = false;
    }
}
