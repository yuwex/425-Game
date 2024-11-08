using System;

public enum Stat : int
{
    ProjectileVelocity,
    ProjectileCooldown,
    ProjectileDamage,
    ProjectileCount,
    TowerRange
}

[Serializable]
public struct StatInfo
{
    public Stat statType;
    public float statValue;
}
