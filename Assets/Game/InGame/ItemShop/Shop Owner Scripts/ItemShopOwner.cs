// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ItemShopOwner : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemShopWindow = default;
    [SerializeField]
    private Talker _beginTalk = default;
    [SerializeField]
    private Talker _endTalk = default;

    [SerializeField]
    private UnityEvent onComplete = default;

    private float _interval = 0.5f;

    private bool _isInterval = false;

    private bool _isPlaying = false;

    public async void Execute(Action onComplete = default)
    {
        if (_isPlaying) return;
        if (_isInterval) return;

        _isPlaying = true;

        // 一言会話してウィンドウを開く
        await _beginTalk.PlayTalk(() => _itemShopWindow.SetActive(true));
        // ウィンドウを閉じる命令が出されたら
        await Wait();
        // ウィンドウを閉じて、一言挨拶して元の状態に戻る
        await _endTalk.PlayTalk(onComplete.Invoke);

        _isPlaying = false;
        onComplete?.Invoke();
        await StartInterval();
    }
    private async UniTask Wait()
    {
        try
        {
            await UniTask.WaitUntil(() => Keyboard.current.escapeKey.wasPressedThisFrame, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        catch (OperationCanceledException)
        {
            return;
        }
        _itemShopWindow.SetActive(false);
    }
    private async UniTask StartInterval()
    {
        _isInterval = true;
        await UniTask.Delay((int)(_interval * 1000f));
        _isInterval = false;
    }
}
