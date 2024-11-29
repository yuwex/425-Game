using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private GameObject target;
    private float fireCountdown = 0f;
    public List<StatInfo> stats = new();

    [Header("Attributes")]

    public int unlockedModifierSlots = 1;
    public int maxModifierSlots = 4;

    public List<StatInfo> baseStats = new();
    public List<ModifierBase> modifiers = new();

    [Header("Setup")]

    public string enemyTag = "Enemy";
    private Vector3 enemyTarget = Vector3.zero;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public delegate List<StatInfo> ModifierHandler(List<StatInfo> mods, List<StatInfo> stats);

    public Dictionary<ModifierType, ModifierHandler> modHandlers = new();


    void Awake()
    {
        modHandlers.Add(ModifierType.Additive, HandleAdditiveModifier);
        modHandlers.Add(ModifierType.Multiplicative, HandleMultiplicativeModifier);
    }

    public List<StatInfo> HandleAdditiveModifier(List<StatInfo> mods, List<StatInfo> stats)
    {

        // Debug.Log("handle");

        var statsDict = stats.ToDictionary(x => x.statType, x => x);

        foreach (var mod in mods)
        {
            if (statsDict.TryGetValue(mod.statType, out var found))
            {
                // Debug.Log("Modded: " + (statsDict[mod.statType] + mod));
                statsDict[mod.statType] += mod;
            }
            else
            {
                statsDict.Add(mod.statType, mod);
            }
        }

        return statsDict.Values.ToList();
    }

    public List<StatInfo> HandleMultiplicativeModifier(List<StatInfo> mods, List<StatInfo> stats)
    {

        var multiplicativeSum = HandleAdditiveModifier(mods, new());

        var statsDict = stats.ToDictionary(x => x.statType, x => x);
        var modsDict = multiplicativeSum.ToDictionary(x => x.statType, x => x);

        foreach (var stat in stats)
        {
            if (modsDict.TryGetValue(stat.statType, out var found))
            {
                statsDict[stat.statType] *= found + new StatInfo{statType = stat.statType, statValue = 1f, modifierType = stat.modifierType};
            }
        }

        return statsDict.Values.ToList();
    }

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);

        UpdateModifiers();
    }

    private void UpdateModifiers() 
    {
        stats = new(baseStats);

        Dictionary<ModifierType, List<StatInfo>> modsDict = new();
        
        // Generate list of all modifiers, organized by ModifierType
        foreach (var mod in modifiers)
        {
            foreach (var stat in mod.statsInfo)
            {
            if (modsDict.ContainsKey(stat.modifierType))
                {
                    modsDict[stat.modifierType].Add(stat);
                }
                else
                {
                    modsDict.Add(stat.modifierType, new List<StatInfo>() {stat});
                }
            }
        }
        
        // Apply each modHandler if it exists
        foreach (var modType in (ModifierType[])Enum.GetValues(typeof(ModifierType)))
        {
            if (modsDict.TryGetValue(modType, out var specificTypeMods))
            {
                if (modHandlers.TryGetValue(modType, out var fn))
                    stats = fn(specificTypeMods, stats);
            }
        }

        // Support for custom modifiers
        foreach (var mod in modifiers)
        {
            foreach (var stat in mod.statsInfo)
            {
                if (stat.modifierType == ModifierType.Custom)
                {
                    GetStat(stat.statType, out float result);
                    var newVal = mod.ModifyCustomStat(stat, result);

                    // Update Value
                    int idx = stats.FindIndex(s => s.statType == stat.statType);
                    if (idx != -1)
                    {
                        var s = stats[idx];
                        s.statValue = newVal;
                        stats[idx] = s;
                    }
                }
            }
        }

        // Prints new stats
        // Debug.Log("New stats: " + string.Join(",", stats));
    }

    public string GetDescription()
    {
        foreach (StatInfo stat in stats.OrderBy(x => x.statType))
        {
            
        }
        
        return "";
    }
    public bool GetStat(Stat type, out float result) 
    {
        foreach (var s in stats) {
            if (s.statType == type) {
                result = s.statValue;
                return true;
            }
        }

        result = 0;
        return false;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToTarget = Vector3.Distance(enemy.transform.position, enemyTarget);
            float distanceFromTower = Vector3.Distance(enemy.transform.position, transform.position);

            GetStat(Stat.TowerRange, out float range);

            if (distanceFromTower < range && distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (fireCountdown <= 0)
        {
            CreateProjectile();
            GetStat(Stat.TowerCooldown, out float cooldown);
            fireCountdown = cooldown;
        }

        fireCountdown -= Time.deltaTime;
    }

    void CreateProjectile()
    {
        
        GetStat(Stat.ProjectileCount, out var count);

        for (int i = 0; i < (int)count; i++)
        {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<TowerProjectile>();

            projectile.stats = stats;
            projectile.modifiers = new(modifiers);
            projectile.target = target;
            projectile.batchCount = (int)count;
            projectile.batchIndex = i;

            projectile.Setup();
        }

    }

    void OnDrawGizmosSelected()
    {
        UpdateModifiers();
        GetStat(Stat.TowerRange, out float range);

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
