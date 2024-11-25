using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private bool damaged = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Pickup") && !damaged)
        {
            damaged = true;
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().Hurt(damage);
            }
            Destroy(gameObject);
        }
    }
}
