using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Pickup"))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().Hurt(damage);
            }
            Destroy(gameObject);
        }
    }
}
