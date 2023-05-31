// 日本語対応
using UnityEngine;

public interface ISavable
{
    public SaveType SaveType { get; }
    public void Save();
    public void Load();
}
public enum SaveType
{
    InGame, OutGame,
}
