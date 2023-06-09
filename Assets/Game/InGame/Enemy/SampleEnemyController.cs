// 日本語対応
using UnityEngine;

public class SampleEnemyController : MonoBehaviour
{
    private Actor_RandomWalk _randomWalk = null;
    private SampleEnemyTracking _tracking = null;
    private SampleEnemyAttack _attack = null;
    private SampleEnemySearch _search = null;

    [SerializeField]
    private EnemyReactionUI _exclamationMark = default;
    [SerializeField]
    private EnemyReactionUI _questionMark = default;

    public EnemyReactionUI ExclamationMark => _exclamationMark;
    public EnemyReactionUI QuestionMark => _questionMark;

    private void Awake()
    {
        _randomWalk = GetComponent<Actor_RandomWalk>();
        _tracking = GetComponent<SampleEnemyTracking>();
        _attack = GetComponent<SampleEnemyAttack>();
        _search = GetComponent<SampleEnemySearch>();

        _randomWalk.enabled = false;
        _tracking.enabled = false;
        _attack.enabled = false;
        _search.enabled = false;
    }
}
