using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public GameObject explosion;
    private bool exploded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Pickup") && !exploded)
        {
            exploded = true;
            GameObject explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
            Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
            HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Enemy") && !hitEnemies.Contains(hit.gameObject))
                {
                    hit.gameObject.GetComponent<Enemy>().Hurt(damage);
                    hitEnemies.Add(hit.gameObject);
                }
            }
            Destroy(explosion, 1);
            Destroy(gameObject);
        }
    }
}
