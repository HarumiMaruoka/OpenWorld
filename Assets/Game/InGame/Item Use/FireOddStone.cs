// 日本語対応
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireOddStone : MonoBehaviour
{
    [SerializeField]
    private float _attackPower = 30f;

    private HashSet<Collider> _colliders = new HashSet<Collider>();

    private void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_colliders.Contains(other)) return;

        if (other.TryGetComponent(out SampleEnemyLife enemyLife))
        {
            enemyLife.Damage(new AttackSet(default, _attackPower, default, default));
        }
        _colliders.Add(other);
    }

    private void OnParticleSystemStopped()
    {
        Debug.LogError("a");
        this.gameObject.SetActive(false);
    }
}
