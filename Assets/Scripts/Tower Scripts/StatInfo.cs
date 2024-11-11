using System;

public enum Stat : int
{
    ProjectileVelocity,
    ProjectileDamage,
    ProjectileCount,
    ProjectileLifetime,
    TowerCooldown,
    TowerRange
}

[Serializable]
public struct StatInfo
{
    public Stat statType;
    public float statValue;
}
