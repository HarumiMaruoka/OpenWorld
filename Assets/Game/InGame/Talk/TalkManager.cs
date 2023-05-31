// 日本語対応
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 現在のシーン用のTextコンポーネントを提供するクラス
/// </summary>
public static class TalkManager
{
    public static HashSet<ITalker> _talkers = new HashSet<ITalker>();
    public static IReadOnlyCollection<ITalker> Talkers => _talkers;

    public static void Register(ITalker talkable)
    {
        _talkers.Add(talkable);
    }
    public static void Lift(ITalker talkable)
    {
        _talkers.Remove(talkable);
    }

    public static bool IsPressedButton()
    {
        return (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)) ||
               (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame);
    }
}
