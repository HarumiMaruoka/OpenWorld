using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

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

    public async UniTask Search(CancellationToken token)
    {
        // 左右に振り向く
        try
        {
            await Rotate(_rotationAmount, _duration, token); // 右へ向く
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token); // ちょっと待つ
            await Rotate(-_rotationAmount * 2, _duration, token); // 左へ向く
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token); // ちょっと待つ
            await Rotate(_rotationAmount, _duration, token); // 正面へ向く
        }
        catch (OperationCanceledException)
        {
            OnSearchCompleted?.Invoke();
            return;
        }
        OnSearchCompleted?.Invoke();
    }

    private async UniTask Rotate(float amount, float duration, CancellationToken token)
    {
        await transform.DORotate(new Vector3(0f, amount, 0f), duration, RotateMode.LocalAxisAdd).
            SetEase(_ease).ToUniTask(cancellationToken: token);
    }
}
