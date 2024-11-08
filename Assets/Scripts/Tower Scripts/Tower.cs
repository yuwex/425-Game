using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
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
    public GameObject bulletPrefab;
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
            Shoot();
            GetStat(Stat.ProjectileCooldown, out float cooldown);
            print(cooldown);
            fireCountdown = cooldown;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        if (bulletGO.TryGetComponent<Bullet>(out var bullet)) {
            bullet.stats = stats;
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        UpdateModifiers();
        GetStat(Stat.TowerRange, out float range);

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
