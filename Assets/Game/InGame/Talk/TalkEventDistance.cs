// 日本語対応
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TalkEventDistance : MonoBehaviour, ITalkEventTrigger
{
    [Header("検知対象。どれか一つでも検知したら会話を実行する。")]
    [SerializeField]
    private Transform[] _targets = default;
    [Header("高さの許容値")]
    [SerializeField]
    private float _heightTolerance = 2f;
    [Header("距離")]
    [SerializeField]
    private float _distance = 5f;
    [Header("会話内容")]
    [SerializeField]
    private Talker _talk = default;

    public void EventTrigger()
    {
        if (WithinDistance(_targets))
        {
            _talk.PlayTalk();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    private bool WithinDistance(Transform[] targets)
    {
        var myPos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);

        for (int i = 0; i < targets.Length; i++)
        {
            var playerPos = new Vector3(PlayerInfo.CurrentPlayerInfo.transform.position.x, 0f, PlayerInfo.CurrentPlayerInfo.transform.position.z);

            if (Mathf.Abs(this.transform.position.y - PlayerInfo.CurrentPlayerInfo.transform.position.y) < _heightTolerance && // 高さの判定
                (myPos - playerPos).sqrMagnitude < _distance * _distance) // x-z平面上の距離の判定)
                return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {

    }
#endif
}
