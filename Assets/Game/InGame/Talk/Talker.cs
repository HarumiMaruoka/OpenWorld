// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Talker : MonoBehaviour, ITalker
{
    [SerializeField]
    private string _name = default;
    [SerializeField]
    private TalkCanvasController _talkCanvasController = default;
    [SerializeField]
    private TalkSet[] _talkEvents = default;

    // 会話機能を重複して他の会話機能は働かせないようにする。
    private static bool _isPlaying = false;

    public string Name => _name;

    private void OnEnable()
    {
        TalkManager.Register(this);
    }
    private void OnDisable()
    {
        TalkManager.Lift(this);
    }

    public async void PlayTalk(Action onComplete = null)
    {
        if (_isPlaying) return;
        _isPlaying = true;
        _talkCanvasController.gameObject.SetActive(true);
        _talkCanvasController.NameText.text = Name;
        for (int i = 0; i < _talkEvents.Length; i++)
        {
            _talkEvents[i].Event?.Invoke();
            _talkCanvasController.TalkText.text = _talkEvents[i].Text;
            await Wait(_talkEvents[i]);
        }
        _talkCanvasController.NameText.text = null;
        _talkCanvasController.TalkText.text = null;
        onComplete?.Invoke();
        _talkCanvasController.gameObject.SetActive(false);
        _isPlaying = false;
    }
    private async UniTask Wait(TalkSet talkProperty)
    {
        await UniTask.Delay((int)(talkProperty.DelayTime * 1000f));
        if (talkProperty.TextUpdateTiming == TextUpdateTiming.InputButton)
        {
            await UniTask.WaitUntil(TalkManager.IsPressedButton);
            return;
        }
        if (talkProperty.TextUpdateTiming == TextUpdateTiming.AnimationEnd)
        {
            await UniTask.WaitUntil(() => _isAnimEnd);
            return;
        }
    }

    private bool _isAnimEnd = false;
    /// <summary>
    /// Animation Event から呼び出すことを想定して作成したメソッド
    /// </summary>
    public async void OnAnimEndForTalk()
    {
        _isAnimEnd = true;
        await UniTask.Yield();
        _isAnimEnd = false;
    }

    [Serializable]
    private class TalkSet
    {
        [Header("表示するテキスト")]
        [TextArea(1, 2)]
        [SerializeField]
        private string _text = default;
        [Header("テキスト表示時のイベント")]
        [SerializeField]
        private UnityEvent _event = default;
        [Header("入力等の無効時間")]
        [SerializeField]
        private float _delayTime = default;
        [Header("会話の更新タイミング")]
        [SerializeField]
        private TextUpdateTiming _textUpdateTiming = default;

        public string Text => _text;
        public UnityEvent Event => _event;
        public float DelayTime => _delayTime;
        public TextUpdateTiming TextUpdateTiming => _textUpdateTiming;
    }
    [Serializable]
    private enum TextUpdateTiming
    {
        InputButton, AnimationEnd, None
    }
}