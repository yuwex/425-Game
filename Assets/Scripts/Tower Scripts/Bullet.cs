using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;

    public float speed = 70f;
    public int damage = 30;

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
        float distance = speed * Time.deltaTime;

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
            enemyHealth.Damage(damage);
        }
        Destroy(gameObject);
    }
}
