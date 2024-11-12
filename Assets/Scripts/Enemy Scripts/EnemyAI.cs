using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform homeBase;    
    public int damageAmount = 10;   
    private NavMeshAgent agent;     
    private double deathDistance = 0; 

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
        if (homeBase != null && agent.remainingDistance <= deathDistance && !agent.pathPending)
        {
            TowerHealth homeHealth = homeBase.GetComponent<TowerHealth>();
            if (homeHealth != null)
            {
                homeHealth.TowerDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }
}
