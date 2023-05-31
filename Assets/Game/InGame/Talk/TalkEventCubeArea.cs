// 日本語対応
using UnityEngine;
using HitSupport;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/// <summary>
/// 四角の範囲にITalkerを検知したらEventTrigger
/// </summary>
public class TalkEventCubeArea : MonoBehaviour, ITalkEventTrigger
{
    [Header("基本的にプレイヤーのレイヤーにヒットさせる事を想定している。")]
    [SerializeField]
    private BoxCast _area = default;
    [Header("会話内容")]
    [SerializeField]
    private Talker _talk = default;

    public void EventTrigger()
    {
        // エリア内のITalkerを検索する。
        if (_area.GetHitInfo(transform).collider != null)
        {
            _talk.PlayTalk();
        }
    }
#if UNITY_EDITOR
    bool isPause = true;
    private void OnDrawGizmosSelected()
    {
        _area.OnDrawGizmos(transform);
    }
#endif
}
