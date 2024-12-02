using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModifier", menuName = "TowerModifiers/Stats")]
public class ModifierBase : ScriptableObject
{
    /*

    All modifiers inherit from a ModifierBase. 
    The modifierbase functions as a object that holds stat values, allows for stat customization, and creates projectile events.
    It's very easy to make your own custom projectiles: just overwrite any of the virtual methods to do so.
    E.g. you can make a projectile that spawns other projectiles on collision.

    There is also a priority system in place for if projectiles should be destroyed.

    The heavy reliance on priority systems ensures that all sorts of different modifiers work together, regardless of their pairings.

    */
    
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
