using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdditiveMaximumModifier", menuName = "TowerModifiers/Additive Maximum")]
public class AdditiveMaximumModifier : ModifierBase
{

    public List<StatInfo> statMaxes;
    public override string DescribeCustomStat(StatInfo info)
    {

        var text = DescribeAdditiveStat(info);

        if (statMaxes.Exists(x => x.statType == info.statType))
        {
            var maxValue = statMaxes.Find(x => x.statType == info.statType).statValue;
            text += $", up to {maxValue}";
        }

        return text;
    }

    public override float ModifyCustomStat(StatInfo info, float currentValue)
    {

        if (statMaxes.Exists(x => x.statType == info.statType))
        {
            var maxValue = statMaxes.Find(x => x.statType == info.statType).statValue;

            if (info.statType == Stat.ProjectileCount && currentValue < maxValue)
                return Mathf.Clamp(currentValue + info.statValue, Mathf.NegativeInfinity, maxValue);
                
        }

        return currentValue;
    }
}
