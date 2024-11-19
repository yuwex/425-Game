using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject mainBase;
    public int waveNumber = 1;
    public int enemyType = 1;
    private int enemiesPerWave = 3;  // Number of enemies per wave
    private float timeBetweenWaves = 10; // Time between waves in seconds
    private float moveSpeed = 5f; // Speed at which the enemy moves towards the center
    private Vector2 spawnRange = new Vector2(4f, 4f); // Range for random X, Y spawn positions

    public List<Transform> spawnPoints;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            // Spawn a wave of enemies
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(enemies[Random.Range(0, enemies.Count)]);
            }

            // Increase the number of enemies per wave and the move speed based on the difficulty level
            enemiesPerWave++;
            moveSpeed += 0.5f;

            // Wait for the specified time before spawning the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        // Randomly spawn the enemy at a random X, Y position within the defined range
        int spawnVal = Random.Range(0, spawnPoints.Count);
   
        // Instantiate the enemy prefab at the spawn position
        Enemy enemy = Instantiate(prefab, spawnPoints[spawnVal].position, Quaternion.identity).GetComponent<Enemy>();
        
        // Resize Enemy
        enemy.transform.localScale = new Vector3(5f, 5f, 5f);
        
        // Set up starting vars
        enemy.target = mainBase;
        enemy.mainBase = mainBase;
    }
}