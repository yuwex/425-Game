using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModifier", menuName = "TowerModifiers/Stats")]
public class ModifierBase : ScriptableObject
{
    public string modifierName;
    public string flavorDescription;
    public string modifierDescription;

    public Material modifierMaterial;
    public List<StatInfo> statsInfo = new();
    protected Dictionary<Stat, float> stats;

    public string description
    {
        get
        {
            var desc = "";

            if (flavorDescription.Length > 0)
                desc += flavorDescription + "\n\n";

            if (modifierDescription.Length > 0)
                desc += modifierDescription + "\n";

            foreach (StatInfo stat in statsInfo.OrderBy(x => ((int)x.statType * 100) + ((int)x.modifierType)))
            {
                var modDesc = "";

                modDesc = stat.modifierType switch
                {
                    ModifierType.Additive => DescribeAdditiveStat(stat),
                    ModifierType.Multiplicative => DescribeMultiplicativeStat(stat),
                    ModifierType.Custom => DescribeCustomStat(stat),
                    _ => "Missing handler for unknown ModifierType",
                };

                desc += modDesc + "\n";
            }

            return desc;
        }
    }

    public virtual float ModifyCustomStat(StatInfo info, float currentValue)
    {
        return currentValue;
    }

    public virtual string DescribeAdditiveStat(StatInfo info)
    {
        var text = info.statType.GetDescription();
        var positive = info.statValue > 0;
        string val = info.statValue.ToString("N0");
        if (positive)
            val = "+" + val;

        if (info.statType == Stat.TowerCooldown)
        {
            positive = !positive;
        }
        
        return text + " <color=" + (positive ?  "green>" : "red>") + val + "</color>";
    }

    public virtual string DescribeMultiplicativeStat(StatInfo info)
    {
        var text = info.statType.GetDescription();
        bool positive = info.statValue > 0;
        string val = (info.statValue * 100).ToString("N0") + "%";
        if (positive) {
            val = "+" + val;
        }

        if (info.statType == Stat.TowerCooldown)
        {
            positive = !positive;
        }
        
        return text + " <color=" + (positive ?  "green>" : "red>") + val + "</color>";
    }

    public virtual string DescribeCustomStat(StatInfo info)
    {
        return "Placeholder for custom text";
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
