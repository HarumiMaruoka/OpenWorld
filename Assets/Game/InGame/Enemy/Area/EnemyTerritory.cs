// 日本語対応
using UnityEngine;

/// <summary>
/// エネミーのテリトリーを表現する
/// </summary>
public class EnemyTerritory : MonoBehaviour
{
    [SerializeField]
    private float _radius = 10f;

    /// <summary>
    /// 指定された座標が、テリトリー内（x-z平面上）に存在するかどうかを取得する。
    /// </summary>
    /// <param name="position"> 検索対象 </param>
    /// <returns> 検索対象がエリア内に存在する場合 trueを返す。 </returns>
    public bool IsInTerritory(Vector3 position)
    {
        Vector3 a = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        Vector3 b = new Vector3(position.x, 0f, position.z);
        return (a - b).sqrMagnitude < _radius * _radius;
    }
    /// <summary>
    /// テリトリー内のランダムな座標を取得する。
    /// </summary>
    /// <returns> テリトリー内のランダムな座標 </returns>
    public Vector3 GetRandomPosition()
    {
        // ランダムな角度を生成
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // ランダムな半径を生成
        float randomRadius = Random.Range(0f, _radius);

        // x-z平面上のランダムな位置を計算
        float x = transform.position.x + Mathf.Cos(angle) * randomRadius;
        float z = transform.position.z + Mathf.Sin(angle) * randomRadius;

        // 新しい座標を返す
        return new Vector3(x, transform.position.y, z);
    }

#if UNITY_EDITOR
    [Header("以下Gizmo関連")]
    [SerializeField]
    private bool _isDrawGizmo = true;
    [SerializeField]
    private Color _gizmoColor = Color.blue;
    [SerializeField]
    private float _height = 1.5f;

    private void OnDrawGizmos()
    {
        if (!_isDrawGizmo) return;

        Gizmos.color = _gizmoColor;
        // エリアの中心位置と高さに基づいて座標を計算する。
        Vector3 centerPosition = this.transform.position;
        float halfHeight = _height * 0.5f;

        // 上部の円の放射状の線を描画する。
        Vector3 topCenter = centerPosition + Vector3.up * halfHeight;
        DrawRadiatingLines(topCenter, _radius);

        // 下部の円の放射状の線を描画する。
        Vector3 bottomCenter = centerPosition - Vector3.up * halfHeight;
        DrawRadiatingLines(bottomCenter, _radius);

        // 円筒の側面部分を描画する。
        DrawCylinder(topCenter, bottomCenter, _radius);
    }
    [SerializeField]
    private int _circleSegmentsCount = 64;
    private void DrawRadiatingLines(Vector3 center, float radius)
    {
        int segments = _circleSegmentsCount; // 放射状の線を描画するためのセグメント数
        float angleIncrement = 360f / segments; // 一つの線の間隔（Degree）

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleIncrement;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Vector3 startPoint = center + direction * radius;
            Gizmos.DrawLine(center, startPoint);
        }
    }
    [SerializeField]
    private int _cylinderSegmentsCount = 64;
    /// <summary> 円筒の側面部分を描画 </summary>
    /// <param name="topCenter"> 上面の中心座標 </param>
    /// <param name="bottomCenter"> 下面の中心座標 </param>
    /// <param name="radius"></param>
    private void DrawCylinder(Vector3 topCenter, Vector3 bottomCenter, float radius)
    {
        int segments = _cylinderSegmentsCount; // 円筒を描画するためのセグメント数
        float angleIncrement = 360f / segments; // 一つの線の間隔（Degree）

        Vector3 topPrevPoint = topCenter + Quaternion.Euler(0f, 0f, 0f) * Vector3.right * radius; // 上部の開始位置を取得
        Vector3 bottomPrevPoint = bottomCenter + Quaternion.Euler(0f, 0f, 0f) * Vector3.right * radius; // 下部の開始位置を取得

        for (int i = 1; i <= segments; i++) // ぐるっと一周 円筒の側面部分を描画する。
        {
            // 角度を取得
            float angle = i * angleIncrement;
            // 上面,下面の座標を取得
            Vector3 topNextPoint = topCenter + Quaternion.Euler(0f, angle, 0f) * Vector3.right * radius;
            Vector3 bottomNextPoint = bottomCenter + Quaternion.Euler(0f, angle, 0f) * Vector3.right * radius;

            // 円筒を描画する。
            Gizmos.DrawLine(topPrevPoint, topNextPoint); // 縦
            Gizmos.DrawLine(topPrevPoint, bottomPrevPoint); // 上部の外周
            Gizmos.DrawLine(bottomPrevPoint, bottomNextPoint); // 下部の外周

            // 今回得た座標を保存しておく。（外周を描画する際に利用する。）
            topPrevPoint = topNextPoint;
            bottomPrevPoint = bottomNextPoint;
        }
    }
#endif
}
