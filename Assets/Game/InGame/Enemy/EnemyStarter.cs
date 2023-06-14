// 日本語対応
using UnityEngine;

public class EnemyStarter : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance.GameStartMode == GameStartMode.Contienue)
        {
            Destroy(this.gameObject);
        }
    }
}
