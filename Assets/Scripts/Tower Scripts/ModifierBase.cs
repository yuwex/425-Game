using System.Collections.Generic;
using UnityEngine;

public class ModifierBase : ScriptableObject
{
    public List<StatInfo> statsInfo = new();
    protected Dictionary<Stat, float> stats;

    protected virtual float ModifyStat(StatInfo stat)
    {
        return stat.statValue;
    }

    public List<StatInfo> ApplyModifiers(List<StatInfo> input) 
    {

        stats = new();
        foreach (var s in statsInfo)
        {
            stats.Add(s.statType, s.statValue);
        }

        List<StatInfo> output = new();

        foreach (var stat in input)
        {

            var nstat = stat;

            if (stats.ContainsKey(stat.statType)) {
                nstat.statValue = ModifyStat(stat);
            }
            
            output.Add(nstat);

        }

        return output;
    }

    public void SetupProjectile(TowerProjectile projectile) {}
    public void UpdateProjectile(TowerProjectile projectile) {}
    public void OnProjectileCollide(TowerProjectile projectile, Collider collider) {}

    // should destroy, priority
    public (bool, int) ShouldDestroyProjectile(TowerProjectile projectile) {
        return (false, -1);
    }

    public void BeforeDestroyProjectile(TowerProjectile projectile) {}

}
