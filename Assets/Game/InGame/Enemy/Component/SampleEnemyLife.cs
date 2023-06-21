// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(EnemyInformation))]
public class SampleEnemyLife : MonoBehaviour, IDamageable
{
    private EnemyInformation _myInfo = null;
    [SerializeField]
    private float _currentLife = float.MaxValue;

    public Transform Transform => transform;
    public float CurrentLife => _currentLife;

    private void Awake()
    {
        _myInfo = GetComponent<EnemyInformation>();
    }
    private void OnEnable()
    {
        DamageableManager.Register(this);
    }
    private void OnDisable()
    {
        DamageableManager.Lift(this);
    }
    private async void Start()
    {
        // 最大ライフを設定する
        _currentLife = (await EnemyDataBase.GetEnemyStatus(_myInfo.MyID, this.GetCancellationTokenOnDestroy())).MaxLife;
    }
    public void SetLife(float value)
    {
        _currentLife = value;
    }
    public void Damage(AttackSet value)
    {
        _currentLife -= value.AttackValue;

        // 死亡時の処理
        if (_currentLife <= 0f)
        {
            Debug.Log($"{gameObject.name} は死亡しました。");
            gameObject.SetActive(false);
        }
    }
}
