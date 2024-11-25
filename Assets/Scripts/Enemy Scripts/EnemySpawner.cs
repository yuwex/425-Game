using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<ModifierBase> possibleModifierDrops;
    public GameObject mainBase;
    public int waveNumber = 1;
    private int enemiesPerWave = 3;
    private float timeBetweenWaves = 15;

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
                yield return new WaitForSeconds(1);
            }

            // Increase the number of enemies per wave and the move speed based on the difficulty level
            enemiesPerWave++;
            GameManager.Instance.enemyWave = waveNumber++;

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

        // Set up starting vars
        enemy.target = mainBase;
        enemy.mainBase = mainBase;

        // Add possible drop
        if (Random.value >= 0.5)
            enemy.modifierDrop = possibleModifierDrops[Random.Range(0, possibleModifierDrops.Count)];
    }
}