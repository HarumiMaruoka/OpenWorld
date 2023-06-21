using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;

public class SampleEnemySearch : MonoBehaviour
{
    [SerializeField]
    private float _duration = 0.6f;
    [SerializeField]
    private float _delay = 0.4f;
    [SerializeField]
    private float _rotationAmount = 90f;
    [SerializeField]
    private Ease _ease = default;

    public event Action OnSearchCompleted = default;

    private void Awake()
    {
        TimeStopManager.IsTimeStop.Subscribe(IsTimeStop).AddTo(this);
    }

    public void Search(CancellationToken token)
    {
        // 左右に振り向く
        Rotate(_rotationAmount, _duration, async () => await Wait(token, () => // 右向いてちょっと待つ
             Rotate(-_rotationAmount * 2, _duration, async () => await Wait(token, () => // 左向いてちょっと待つ
             Rotate(_rotationAmount, _duration, () => OnSearchCompleted?.Invoke()))))); // 正面向く
    }

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _currentAnim = null;

    public void Kill()
    {
        _currentAnim?.Kill();
    }

    private async void Rotate(float amount, float duration, Action onComplete = null)
    {
        await UniTask.WaitUntil(() => !TimeStopManager.IsTimeStop.Value);
        _currentAnim = transform.DORotate(new Vector3(0f, amount, 0f), duration, RotateMode.LocalAxisAdd).
            SetEase(_ease).OnComplete(() => { _currentAnim = null; onComplete?.Invoke(); });
    }

    private async UniTask Wait(CancellationToken token, Action onComplete)
    {
        float timer = 0f;
        while (timer > _delay)
        {
            if (TimeStopManager.IsTimeStop == null) return;
            if (!TimeStopManager.IsTimeStop.Value)
                timer += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
        }
        onComplete?.Invoke();
    }

    private void IsTimeStop(bool value)
    {
        if (_currentAnim != null)
        {
            if (value) _currentAnim.Pause();
            else _currentAnim.Play();
        }
    }
}
