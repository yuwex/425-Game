using UnityEngine;

[CreateAssetMenu(fileName = "AdditiveModifier", menuName = "TowerModifiers/Additive")]
public class AdditiveModifier : ModifierBase
{
    protected override float ModifyStat(StatInfo stat)
    {
        if (stats.TryGetValue(stat.statType, out float found))
        {
            return stat.statValue + found;
        }

        return stat.statValue;
    }
}
