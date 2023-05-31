// 日本語対応

/// <summary>
/// アイテムの種類を表す列挙型
/// </summary>
public enum ItemUseTargetType
{
    /// <summary>
    /// 使用者自身に対して効果を適用する。
    /// </summary>
    ApplyEffectToSelf,
    /// <summary>
    /// 特定の相手に対して効果を適用する。
    /// </summary>
    ApplyEffectToTarget,
    /// <summary>
    /// エリア内の相手に対して効果を適用する。
    /// </summary>
    ApplyEffectToArea,
    /// <summary>
    /// シーンに存在する全ての相手に対して効果を適用する。
    /// </summary>
    ApplyEffectToAll
}