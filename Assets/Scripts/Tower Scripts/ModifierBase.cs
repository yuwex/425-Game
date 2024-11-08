using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierBase : ScriptableObject
{
    public List<StatInfo> statsInfo = new();
    protected Dictionary<Stat, float> stats;

    protected abstract float ModifyStat(StatInfo stat);

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

    public void SetupBullet() {}
    public void UpdateBullet() {}
    public void DestroyBullet() {}

}
