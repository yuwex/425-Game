using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


[CreateAssetMenu(fileName = "AdditiveMaximumModifier", menuName = "TowerModifiers/Additive Maximum")]
public class AdditiveMaximumModifier : ModifierBase
{

    public List<StatInfo> statMaxes;
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
