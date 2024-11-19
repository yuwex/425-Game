using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType : int
{
    Normal,
}

public enum EnemyGroup : int
{
    Normal,
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public float baseHealth;
    public float speed;
    public float attackDamage;
    public float baseDamage;
    public int coinReward;
    
    public void SetupNavMeshAgent(NavMeshAgent agent)
    {
        agent.speed = speed;
    }
}
