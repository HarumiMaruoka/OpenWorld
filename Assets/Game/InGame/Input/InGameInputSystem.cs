// 日本語対応
using UnityEngine;

public class InGameInputSystem : MonoBehaviour
{
    // EventSystemを参考に入力管理クラスを作るぞ
    // 他のプロジェクトに移植する際は、[Project Settings]の
    // [Script Execution Order]を変更し、
    // どのコンポーネントよりも先に呼ばれるように変更する。
    private static InGameInputSystem _current = null;
    public static InGameInputSystem Current => _current;

    private InGameInput _inputActions = null;
    public InGameInput InputActions => _inputActions;

    private void Awake()
    {
        _current = this;
        _inputActions = new InGameInput();
        _inputActions.Enable();
    }
}
