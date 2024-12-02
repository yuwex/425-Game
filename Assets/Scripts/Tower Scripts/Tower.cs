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
    public List<int> modifierSlotCosts;

    public List<StatInfo> baseStats = new();
    public List<ModifierBase> modifiers = new();

    [Header("Setup")]

    public string enemyTag = "Enemy";
    private Vector3 enemyTarget = Vector3.zero;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public delegate List<StatInfo> ModifierHandler(List<StatInfo> mods, List<StatInfo> stats);

    public Dictionary<ModifierType, ModifierHandler> modHandlers = new();

    public Dictionary<Stat, string> statChangeDescriptions = new();
    public string _statChangeDescOut = "";


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

    public void UpdateModifiers() 
    {
        stats = new(baseStats);

        Dictionary<ModifierType, List<StatInfo>> modsDict = new();
        List<(ModifierType, List<StatInfo>)> statChanges = new(); 

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
                {
                    var updated = fn(specificTypeMods, stats);
                    statChanges.Add((modType, updated));
                    stats = updated;

                }
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

        statChanges.Add((ModifierType.Custom, stats));

        // Generate Descriptions from Changes

        // Turn above datastruct into this one
        Dictionary<Stat, List<(ModifierType, float)>> changes = new();
        foreach ((ModifierType t, List<StatInfo> st) in statChanges)
        {
            foreach (StatInfo s in st)
            {
                if (changes.TryGetValue(s.statType, out var found))
                {
                    found.Add((t, s.statValue));
                }
                else
                {
                    changes.Add(s.statType, new List<(ModifierType, float)> {(t, s.statValue)});
                }
            }
        }

        // Generate text
        _statChangeDescOut = "";
        statChangeDescriptions = new();
        foreach(Stat s in changes.Keys)
        {
            GetBaseStat(s, out var baseval);
            changes.TryGetValue(s, out var values);
            float cval = baseval;
            
            List<(string, float)> operations = new();
            
            foreach ((ModifierType t, float v) in values)
            {
                if (v == cval) continue;

                switch (t)
                {
                    case ModifierType.Custom:
                    case ModifierType.Additive:
                        operations.Add(("+", v - cval));
                        break;
                    
                    case ModifierType.Multiplicative:
                        operations.Add(("*", v / cval));
                        break;
                    
                    default:
                        break;
                }

                cval = v;
            }

            string desc = "";
            if (operations.Count == 0)
            {
                desc = "Base Value";
            }
            else
            {
                desc = baseval + "";

                foreach ((string op, float v) in operations)
                {
                    string _op = op;
                    float _v = v;
                    bool isPositive = true;

                    if (op == "+") isPositive = v > 0;
                    if (op == "*") isPositive = v > 1;
                    if (s == Stat.TowerCooldown) isPositive = !isPositive;
                    
                    
                    if (op == "+" && v < 0)
                    {
                        _op = "-";
                        _v = Mathf.Abs(v);
                    }

                    string color = isPositive ? "green" : "red";

                    desc = $"({desc} {_op} <color={color}>{_v}</color>)";
                }

                char[] toTrim = {'(',')'};
                desc = desc.Trim(toTrim);
            }

            _statChangeDescOut += $"{s} {desc}\n";
            statChangeDescriptions[s] = desc;
        }

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

    public bool GetBaseStat(Stat type, out float result) 
    {
        foreach (var s in baseStats) {
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
