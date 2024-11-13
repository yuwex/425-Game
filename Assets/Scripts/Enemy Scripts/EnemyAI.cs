using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform homeBase;    
    private NavMeshAgent agent;
    public int damageAmount;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (homeBase != null)
        {
            agent.SetDestination(homeBase.position);
        }
    }

    void Update()
    {
        if (homeBase == null)
        {
            Destroy(gameObject);
        } 
    }

    private void OnTriggerEnter(Collider collision)
    {
        TowerHealth health = collision.gameObject.GetComponent<TowerHealth>();
        if (health != null && collision.gameObject.tag == "Base")
        {
            health.TowerDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
