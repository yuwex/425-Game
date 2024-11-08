using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;

    public List<StatInfo> stats = new();

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

    public void Seek(GameObject target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        GetStat(Stat.ProjectileVelocity, out float velocity);
        float distance = velocity * Time.deltaTime;

        if (direction.magnitude <= distance)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distance, Space.World);

    }

    void HitTarget()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null) {
            GetStat(Stat.ProjectileDamage, out float damage);

            enemyHealth.Damage((int)damage);
        }
        Destroy(gameObject);
    }
}
