using System;
using System.ComponentModel;
using System.Diagnostics;

public enum Stat : int
{
    [Description("Projectile Damage")]
    ProjectileDamage,

    [Description("Projectile Speed")]
    ProjectileVelocity,

    [Description("Projectile Lifetime")]
    ProjectileLifetime,

    [Description("Projectiles")]
    ProjectileCount,

    [Description("Tower Cooldown")]
    TowerCooldown,
    
    [Description("Tower Range")]
    TowerRange
}

public enum ModifierType : int
{
    Additive = 1,
    Multiplicative = 2,
    Custom = 3
}

[Serializable]

public struct StatInfo
{
    public Stat statType;
    public float statValue;
    public ModifierType modifierType;

    public static StatInfo operator + (StatInfo a, StatInfo b) 
    {
        if (a.statType != b.statType)
        {
            throw new Exception("Unable to add StatInfo objects of different types");
        }

        return new StatInfo() {statType = a.statType, statValue = a.statValue + b.statValue, modifierType = a.modifierType};
    }

    public static StatInfo operator * (StatInfo a, StatInfo b) 
    {
        if (a.statType != b.statType)
        {
            throw new Exception("Unable to multiply StatInfo objects of different types");
        }

        return new StatInfo() {statType = a.statType, statValue = a.statValue * b.statValue, modifierType = a.modifierType};
    }

    public override string ToString()
    {
        return $"[statType = {statType}, statValue = {statValue}, modifierType = {modifierType}]";


    }
}