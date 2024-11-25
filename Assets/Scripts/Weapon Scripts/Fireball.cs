using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public GameObject explosion;

    private void OnTriggerEnter()
    {
        GameObject explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                hit.gameObject.GetComponent<Enemy>().Hurt(damage);
            }
        }
        Destroy(explosion, 1);
        Destroy(gameObject);
    }
}
