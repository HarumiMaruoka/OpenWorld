// 日本語対応
using UnityEngine;

public class FieldObjectUI : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
