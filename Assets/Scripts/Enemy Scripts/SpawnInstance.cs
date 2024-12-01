using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Instance", menuName = "Spawn Instance")]
public class SpawnInstance : ScriptableObject
{
    public EnemyData enemyData;
    public GameObject enemyPrefab;
    public int quantity;
}
