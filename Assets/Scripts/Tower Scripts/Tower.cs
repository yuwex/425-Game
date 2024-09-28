using UnityEngine;

public class Tower : MonoBehaviour
{

    private Transform target;

    [Header("Attributes")]
    public float range = 20f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Setup")]

    public string enemyTag = "Enemy";
    private UnityEngine.Vector3 enemyTarget = UnityEngine.Vector3.zero;
    public GameObject bulletPrefab;
    public Transform firePoint;


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToTarget = UnityEngine.Vector3.Distance(enemy.transform.position, enemyTarget);
            float distanceFromTower = UnityEngine.Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceFromTower < range && distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
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
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null) {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
