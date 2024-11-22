using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModifier", menuName = "TowerModifiers/Stats")]
public class ModifierBase : ScriptableObject
{

    public Material modifierMaterial;
    public List<StatInfo> statsInfo = new();
    protected Dictionary<Stat, float> stats;

    public virtual float ModifyCustomStat(StatInfo info, float currentValue)
    {
        return currentValue;
    }

    public virtual void SetupProjectile(TowerProjectile projectile) {}
    public virtual void UpdateProjectile(TowerProjectile projectile) {}
    public virtual void OnProjectileCollide(TowerProjectile projectile, Collider collider) {}

    // should destroy, priority
    public virtual (bool, int) ShouldDestroyProjectile(TowerProjectile projectile) {
        return (false, -1);
    }

    public virtual void BeforeDestroyProjectile(TowerProjectile projectile) {}

}
