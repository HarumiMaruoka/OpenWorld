// 日本語対応
using UnityEngine;
using HitSupport;
using Cysharp.Threading.Tasks;

public class PlayerTalk : MonoBehaviour, ITalkEventTrigger
{
    [SerializeField]
    private Raycast _raycast;
    [SerializeField]
    private float _talkInterval = 0.4f;

    private PlayerInfo _info = null;
    private bool _isTalkInterval = false;

    private void Start()
    {
        _info = GetComponent<PlayerInfo>();
    }
    private void Update()
    {
        EventTrigger();
    }
    public void EventTrigger()
    {
        if (_isTalkInterval) return;
        // 入力が発生したとき、前方にITalkableが存在したら会話プログラムを実行する。
        if (TalkManager.IsPressedButton() && ForwardConfirmation(out ITalker talker))
        {
            _info.AddState(PlayerState.Talk);
            talker.PlayTalk(TalkEnd);
        }
        else if (TalkManager.IsPressedButton() && ForwardConfirmation(out ItemShopOwner shopOwner))
        {
            _info.AddState(PlayerState.Talk);
            shopOwner.Execute(TalkEnd);
        }
    }
    private async void TalkEnd()
    {
        _info.RemoveState(PlayerState.Talk);
        _isTalkInterval = true;
        await UniTask.Delay((int)(_talkInterval * 1000f));
        _isTalkInterval = false;
    }
    private bool ForwardConfirmation<T>(out T talker) where T : class
    {
        RaycastHit raycastHit;
        _raycast.IsHit(transform, out raycastHit);
        if (raycastHit.collider != null)
        {
            raycastHit.collider.TryGetComponent(out T rayTalker);
            talker = rayTalker;
        }
        else
        {
            talker = null;
        }
        return talker != null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        _raycast.OnDrawGizmo(transform);
    }
#endif
}
