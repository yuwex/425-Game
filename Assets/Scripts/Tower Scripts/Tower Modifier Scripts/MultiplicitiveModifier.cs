using UnityEngine;

[CreateAssetMenu(fileName = "MultiplicitiveModifier", menuName = "TowerModifiers/Multiplicitive")]
public class MultiplicitiveModifier : ModifierBase
{
    protected override float ModifyStat(StatInfo stat)
    {
        if (stats.TryGetValue(stat.statType, out float found))
        {
            return stat.statValue * found;
        }

        return stat.statValue;
    }
}
