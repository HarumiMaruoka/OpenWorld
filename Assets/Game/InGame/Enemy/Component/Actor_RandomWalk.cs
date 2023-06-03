// 日本語対応
using UnityEngine;

public class Actor_RandomWalk : MonoBehaviour
{
    [Header("座標の取得範囲。（自分の座標からの半径）"), SerializeField]
    private float _radius = 3f;
    [Header("座標を取得する際、その座標が適切かどうか判定する用")]
    [Header("判定用の高さオフセット"), SerializeField]
    private float _heightOffset = 3f;
    [Header("崖検知高さ"), SerializeField]
    private float cliffDetectionHeight = 5f;
}
