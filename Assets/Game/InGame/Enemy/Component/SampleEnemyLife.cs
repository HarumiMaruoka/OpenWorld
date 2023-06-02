// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(EnemyInformation))]
public class SampleEnemyLife : MonoBehaviour, IDamageable
{
    private EnemyInformation _myInfo = null;
    [SerializeField]
    private float _currentLife = float.MaxValue;

    private void Awake()
    {
        _myInfo = GetComponent<EnemyInformation>();
    }
    private async void Start()
    {
        // 最大ライフを設定する
        _currentLife = (await EnemyDataBase.GetEnemyStatus(_myInfo.MyID, this.GetCancellationTokenOnDestroy())).MaxLife;
    }
    public void Damage(float value)
    {
        _currentLife -= value;

        // 死亡時の処理
        if (_currentLife <= 0f)
        {
            Debug.Log($"{gameObject.name} は死亡しました。");
            gameObject.SetActive(false);
        }
    }
}
