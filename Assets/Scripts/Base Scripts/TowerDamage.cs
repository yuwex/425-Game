using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDamage : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider collision)
    {
        TowerHealth health = collision.gameObject.GetComponent<TowerHealth>();
        if (health != null && collision.gameObject.tag == "Base")
        {
            health.TowerDamage(damage);
        }
    }
}