// 日本語対応
using UnityEngine;

public class SampleEnemyExplosionEndProcessing : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(this.gameObject);
    }
}
