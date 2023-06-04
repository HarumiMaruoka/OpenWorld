// 日本語対応
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SampleEnemyController : MonoBehaviour
{
    private EnemyStateMachine _enemyStateMachine = default;
    private EnemySight _enemySight = default;
    private SampleEnemyTracking _sampleEnemyTracking = default;
    private float _timer = 0f;
    private CancellationTokenSource _trackingTokenSource = null;

    private void Start()
    {
        //_enemyStateMachine = GetComponent<EnemyStateMachine>();
        //_enemySight = GetComponent<EnemySight>();
        //_sampleEnemyTracking = GetComponent<SampleEnemyTracking>();
        //_enemySight.IsFind.Subscribe(OnSightStateChanged);
    }

    private void OnSightStateChanged(bool found)
    {
        if (found)
        {
            _trackingTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            _sampleEnemyTracking.Tracking(PlayerInfo.CurrentPlayerInfo.transform, _trackingTokenSource.Token);
        }
        else
        {
            StopTracking();
        }
    }

    private void StopTracking()
    {
        if (_trackingTokenSource != null)
        {
            _sampleEnemyTracking.LostSightOf();
            _trackingTokenSource.Cancel();
            _trackingTokenSource.Dispose();
            _trackingTokenSource = null;
        }
    }

    private void OnDestroy()
    {
        StopTracking();
    }
}
