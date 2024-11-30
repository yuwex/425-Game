using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<ModifierBase> possibleModifierDrops;
    public GameObject mainBase;
    public int waveNumber = 0;
    private int enemiesPerWave = 3;
    // private float timeBetweenWaves = 15;
    public bool betweenWaves;

    public List<Transform> spawnPoints;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            betweenWaves = true;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));

            betweenWaves = false;
            // Increase the number of enemies per wave and the move speed based on the difficulty level
            GameManager.Instance.enemyWave = ++waveNumber;

            // Spawn a wave of enemies
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(enemies[Random.Range(0, enemies.Count)]);
                yield return new WaitForSeconds(1);
            }

            // Wait for the specified time before spawning the next wave
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
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