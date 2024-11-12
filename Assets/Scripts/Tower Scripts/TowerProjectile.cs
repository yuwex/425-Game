using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public GameObject target;

    public List<ModifierBase> modifiers = new();

    public List<StatInfo> stats = new();

    public int batchIndex;
    public int batchCount;

    public float deleteAt = float.MaxValue;

    private bool anyEnemyHit = false;

    public List<GameObject> enemiesHit = new();

    public static bool CalculateLateralTrajectory(Vector3 pos, Vector3 target, float velocity, float maxHeight, out Vector3 force, out Vector3 gravity) {
    
        // Adapted from https://www.forrestthewoods.com/blog/solving_ballistic_trajectories/
        // Credit: Forest Smith
        
        force = Vector3.zero;
        gravity = Vector3.zero;

        Vector3 diff = target - pos;
        Vector3 diffXZ = new(diff.x, 0f, diff.z);
        float lateralDist = diffXZ.magnitude;

        if (lateralDist == 0)
            return false;

        float time = lateralDist / velocity;

        force = diffXZ.normalized * velocity;

        float a = pos.y;        // initial
        float b = maxHeight;    // peak
        float c = target.y;     // final

        gravity.y = 4*(a - 2*b + c) / (time * time);
        force.y = -(3*a - 4*b + c) / time;

        return true;
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

    void Start()
    {

        // Center of target
        Vector3 targetPos = target.GetComponent<Renderer>().bounds.center;

        // Get Velocity
        GetStat(Stat.ProjectileVelocity, out var velocity);

        // Find fancy arc
        CalculateLateralTrajectory(transform.position, targetPos, velocity, 5f, out var force, out var gravity);
        
        // Set custom gravity and force
        GetComponent<ConstantForce>().force = gravity;
        GetComponent<Rigidbody>().velocity = force;

        // Ensure modifier setups run after all setup
        foreach (var mod in modifiers)
        {
            mod.SetupProjectile(this);
        }

    }

    void Setup()
    {

        GetStat(Stat.ProjectileLifetime, out var lifetime);

        deleteAt = Time.time + lifetime;

        foreach (var mod in modifiers)
        {
            if (mod == null) continue;
            mod.SetupProjectile(this);
        }
    }

    void Update()
    {
        // Run all modifiers update methods
        foreach (var mod in modifiers) 
        {
            if (mod == null) continue;
            mod.UpdateProjectile(this);
        }

        if (ShouldDestroy())
        {

            // Call pre-death modifiers
            foreach (var mod in modifiers)
            {
                if (mod == null) continue;
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
            if (mod == null) continue;
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
            if (mod == null) continue;
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
                other.gameObject.GetComponent<EnemyHealth>().Damage(damage);
            }
            
        }

    }
}
