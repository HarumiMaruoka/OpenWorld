// 日本語対応
using UnityEngine;

/// <summary>
/// 全ての敵に共通して使用する情報を提供する
/// </summary>
public class EnemyInformation : MonoBehaviour
{
    [SerializeField]
    private int _myID = -1;
    public int MyID => _myID;

    [SerializeField]
    private SampleEnemyLife _life = default;

    public void Load(Vector3 position, float value)
    {
        transform.position = position; _life.SetLife(value);
    }
}