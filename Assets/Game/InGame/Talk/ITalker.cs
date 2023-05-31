// 日本語対応

using System;
/// <summary>
/// 話相手を表現する
/// </summary>
public interface ITalker
{
    public string Name { get; }
    public void PlayTalk(Action onComplete = null);
}
