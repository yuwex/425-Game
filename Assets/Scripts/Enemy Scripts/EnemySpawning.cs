using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject enemyObject;

    private int enemiesPerWave = 3;  // Number of enemies per wave
    private float timeBetweenWaves = 10; // Time between waves in seconds
    private float moveSpeed = 5f; // Speed at which the enemy moves towards the center
    private Vector2 spawnRange = new Vector2(4f, 4f); // Range for random X, Y spawn positions

    private Vector3 targetPosition = Vector3.zero; // target position

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

     IEnumerator SpawnWaves()
    {
        while (true){
            // Spawn a wave of enemies
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
            }

            // Increase the number of enemies per wave and the move speed based on the difficulty level
            enemiesPerWave++;
            moveSpeed += 0.5f;

            // Wait for the specified time before spawning the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        // Randomly spawn the enemy at a random X, Y position within the defined range
        Vector3 spawnPosition = new Vector3(Random.Range(-4, 4)*25, 0f, Random.Range(-4, 4)*25);

        // Instantiate the enemy prefab at the spawn position
        GameObject enemy = Instantiate(enemyObject, spawnPosition, Quaternion.identity);
        enemy.transform.localScale = new Vector3(5f, 5f, 5f);
        EnemyMover mover = enemy.AddComponent<EnemyMover>();
        mover.moveSpeed = moveSpeed;
    }
}
