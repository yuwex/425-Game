using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private GameObject target;
    private float fireCountdown = 0f;
    private List<StatInfo> stats = new();

    [Header("Attributes")]

    public List<StatInfo> baseStats = new();
    public List<ModifierBase> modifiers = new();

    [Header("Setup")]

    public string enemyTag = "Enemy";
    private Vector3 enemyTarget = Vector3.zero;
    public GameObject projectilePrefab;
    public Transform firePoint;


    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);

        UpdateModifiers();
    }

    private void UpdateModifiers() 
    {
        stats = new(baseStats);
        foreach (var mod in modifiers)
        {
            stats = mod.ApplyModifiers(stats);
        }
    }

    private bool GetStat(Stat type, out float result) 
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

            foreach (var mod in projectile.modifiers)
            {
                mod.SetupProjectile(projectile);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        UpdateModifiers();
        GetStat(Stat.TowerRange, out float range);

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
