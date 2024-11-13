using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform homeBase;    
    private NavMeshAgent agent;     

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
}
