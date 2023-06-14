// 日本語対応
using UnityEngine;

public class ItemShopTester : MonoBehaviour
{
    [SerializeField]
    private ItemShopOwner _shopOwner = default;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shopOwner.Execute();
        }
    }
}
