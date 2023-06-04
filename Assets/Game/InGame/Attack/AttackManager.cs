using System;
using System.Collections.Generic;
using UnityEngine;

public static class AttackManager
{
    public static void Attack(AttackSet attackSet, IDamageable damageable)
    {
        if (!IsWithinAttackRange(attackSet, damageable))
        {
            return;
        }

        damageable.Damage(attackSet);
    }

    private static bool IsWithinAttackRange(AttackSet attackSet, IDamageable damageable)
    {
        // 攻撃範囲内にいるかどうかの判定ロジックを実装する
        // 必要に応じて、攻撃範囲の形状や距離に基づいて計算を行う
        // 例: プレイヤーとの距離が一定範囲内か、攻撃範囲内にプレイヤーが含まれるかなど
        // 判定結果を返す
        return default; // コンパイルエラー回避用の仮置きコード
    }
}

public struct AttackSet
{
    public AttackSet(AttackType type, float value, AttackArea area, AttackEffect effect)
    {
        _attackType = type;
        _attackValue = value;
        _attackArea = area;
        _attackEffect = effect;
    }

    private readonly AttackType _attackType;
    private readonly float _attackValue;
    private readonly AttackArea _attackArea;
    private readonly AttackEffect _attackEffect;

    public AttackType AttackType => _attackType;
    public float AttackValue => _attackValue;
    public AttackArea AttackArea => _attackArea;
    public AttackEffect AttackEffect => _attackEffect;
}

public enum AttackType
{
    // 攻撃の種類を定義する
    // 必要に応じて具体的な攻撃種類を列挙する
}

public abstract class AttackArea
{
    // 攻撃範囲の基底クラス
    // 必要に応じて具体的な攻撃範囲のクラスを継承して実装する
}

public abstract class AttackEffect
{
    // 攻撃効果の基底クラス
    // 必要に応じて具体的な攻撃効果のクラスを継承して実装する
}

public interface IDamageable
{
    void Damage(AttackSet attackSet);
}

public static class DamageableManager
{
    private static HashSet<IDamageable> _damageableSet = new HashSet<IDamageable>();

    public static IReadOnlyCollection<IDamageable> DamageableCollection => _damageableSet;

    public static void Register(IDamageable damageable)
    {
        _damageableSet.Add(damageable);
    }

    public static void Lift(IDamageable damageable)
    {
        _damageableSet.Remove(damageable);
    }
}
