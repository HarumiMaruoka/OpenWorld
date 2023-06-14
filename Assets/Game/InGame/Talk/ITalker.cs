// 日本語対応

using Cysharp.Threading.Tasks;
using System;
/// <summary>
/// 話相手を表現する
/// </summary>
public interface ITalker
{
    public string Name { get; }
    public UniTask PlayTalk(Action onComplete = null);
}
