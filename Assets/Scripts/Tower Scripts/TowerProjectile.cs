using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public GameObject target;

    public List<ModifierBase> modifiers = new();

    public List<StatInfo> stats = new();

    public int batchIndex;
    public int batchCount;

    private float deleteAt = float.MaxValue;

    private bool anyEnemyHit = false;

    private List<GameObject> enemiesHit = new();

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

    void Start()
    {
        // Center of target
        Vector3 targetPos = target.GetComponent<Renderer>().bounds.center;

        // Direction to center of target
        Vector3 dir = (targetPos - transform.position).normalized;

        // Get Velocity
        GetStat(Stat.ProjectileVelocity, out var velocity);

        // Add force
        GetComponent<Rigidbody>().AddForce(dir * velocity, ForceMode.Impulse);
    }

    void Setup()
    {

        GetStat(Stat.ProjectileLifetime, out var lifetime);

        deleteAt = Time.time + lifetime;

        foreach (var mod in modifiers)
        {
            mod.SetupProjectile(this);
        }
    }

    void Update()
    {
        // Run all modifiers update methods
        foreach (var mod in modifiers) 
        {
            mod.UpdateProjectile(this);
        }

        if (ShouldDestroy())
        {

            // Call pre-death modifiers
            foreach (var mod in modifiers)
            {
                mod.BeforeDestroyProjectile(this);
            }

            Destroy(gameObject);
        }
    }

    bool ShouldDestroy()
    {
        // Checks if modifiers have specific priority-based conditions for death

        // Highest priority: death time reached
        if (Time.time > deleteAt)
        {
            return true;
        }

        var die = false;
        var weight = 0;

        // Override death modifiers using priority system
        foreach (var mod in modifiers)
        {
            var (cdie, cweight) = mod.ShouldDestroyProjectile(this);
            if (cweight >= weight) {
                die = cdie;
                weight = cweight;
            }
        }

        // If no modifiers to death, default to hit 1 enemy
        if (weight == 0 && anyEnemyHit) 
        {
            die = true;
        }

        return die;
    }

    private void OnTriggerEnter(Collider other) 
    {
        // Let all modifiers deal with trigger first
        foreach (var mod in modifiers)
        {
            mod.OnProjectileCollide(this, other);
        }

        if (other.gameObject.CompareTag("Enemy")) 
        {
            // Used to check if any enemy has been hit (for default deletion scheme)
            if (!anyEnemyHit)
            {
                anyEnemyHit = true;
            }

            // Damage scheme: deal damage to an enemy exactly once
            if (!enemiesHit.Contains(other.gameObject)) 
            {
                enemiesHit.Add(other.gameObject);

                GetStat(Stat.ProjectileDamage, out float damage);
                other.gameObject.GetComponent<EnemyHealth>().Damage((int) damage);
            }
            
        }

    }
}
