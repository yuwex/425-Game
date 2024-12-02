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

    public List<Wave> waves;
    public WaveTimeLeft counter;
    public WaveWarning waveWarning;


    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        betweenWaves = true;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        betweenWaves = false;
        while (waveNumber < waves.Count)
        {
            GameManager.Instance.enemyWave = ++waveNumber;
            waveWarning.ShowWarning("Wave " + waveNumber);

            List<int> unassignedSpawns = new List<int> { 0, 1, 2, 3 };
            int[] spawnVals = new int[4];
            for (int i = 0; i < 4; i++)
            {
                spawnVals[i] = Random.Range(0, unassignedSpawns.Count);
                unassignedSpawns.Remove(spawnVals[i]);
            }

            StartCoroutine(SpawnGroup(waves[waveNumber - 1].group1, spawnVals[0]));
            StartCoroutine(SpawnGroup(waves[waveNumber - 1].group2, spawnVals[1]));
            StartCoroutine(SpawnGroup(waves[waveNumber - 1].group3, spawnVals[2]));
            StartCoroutine(SpawnGroup(waves[waveNumber - 1].group4, spawnVals[3]));

            counter.StartCountdown(waves[waveNumber - 1].nextRoundDelay);
            yield return new WaitForSeconds(waves[waveNumber - 1].nextRoundDelay);
        }

        //endless mode
        while (true)
        {
            betweenWaves = true;
            counter.StartCountdown(30);
            yield return new WaitForSeconds(30);
            betweenWaves = false;
            waveWarning.ShowWarning("Wave " + waveNumber);
            GameManager.Instance.enemyWave = ++waveNumber;

            // Spawn a wave of enemies
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(enemies[Random.Range(0, enemies.Count)], Random.Range(0, spawnPoints.Count));
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void SpawnEnemy(GameObject prefab, int spawnVal)
    {

        // Instantiate the enemy prefab at the spawn position
        Enemy enemy = Instantiate(prefab, spawnPoints[spawnVal].position, Quaternion.identity).GetComponent<Enemy>();

        // Set up starting vars
        enemy.target = mainBase;
        enemy.mainBase = mainBase;

        // Add possible drop
        if (Random.value >= 0.5f)
            enemy.modifierDrop = possibleModifierDrops[Random.Range(0, possibleModifierDrops.Count)];
    }

    IEnumerator SpawnGroup(List<SpawnInstance> group, int spawnVal)
    {
        foreach (SpawnInstance spawnInstance in group)
        {
            for (int i = 0; i < spawnInstance.quantity; i++)
            {
                Enemy enemy = Instantiate(spawnInstance.enemyPrefab, spawnPoints[spawnVal].position, Quaternion.identity).GetComponent<Enemy>();
                enemy.data = spawnInstance.enemyData;
                enemy.target = mainBase;
                enemy.mainBase = mainBase;
                if (Random.value >= 0.95f)
                    enemy.modifierDrop = possibleModifierDrops[Random.Range(0, possibleModifierDrops.Count)];
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}