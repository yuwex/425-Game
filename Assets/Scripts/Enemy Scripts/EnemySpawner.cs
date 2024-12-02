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

    private bool started = false;

    public List<Transform> spawnPoints;

    public List<Wave> waves;
    public WaveTimeLeft counter;
    public WaveWarning waveWarning;
    public PlayerInventory inventory;

    [Header("Victory Stuff")]
    public GameObject victoryScreen;
    public GameObject playerCamera;
    public GameObject winningCamera;
    public GameObject ui;


    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (!started && Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameManager.Instance.enemyWave = ++waveNumber;
            GameManager.Instance.playerCoins += MoneyFromWave(waves[waveNumber-1]);
            inventory.Add(possibleModifierDrops[Random.Range(0, possibleModifierDrops.Count)]);
        }
        if (!started && Input.GetKeyDown(KeyCode.DownArrow) && waveNumber > 0)
        {
            GameManager.Instance.enemyWave = --waveNumber;
            GameManager.Instance.playerCoins -= MoneyFromWave(waves[waveNumber]);
        }
    }

    /* Our ScriptableObject based wave system is noteworthy because it allows for 
    the creation of waves that feel disctinct and require different strategies, 
    promoting a flow state within the user. */

    IEnumerator SpawnWaves()
    {
        betweenWaves = true;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        started = true;
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

        ui.GetComponent<Canvas>().enabled = false;
        VictoryText victoryTextScript = victoryScreen.GetComponent<VictoryText>();
        victoryTextScript.displayNewText();
        victoryScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerCamera.SetActive(false);
        winningCamera.SetActive(true);
        
        // ui.GetComponent<Canvas>().enabled = true;

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

    int MoneyFromWave(Wave wave)
    {
        int res = 0;
        foreach (SpawnInstance spawn in wave.group1)
        {
            res += spawn.enemyData.coinReward * spawn.quantity;
        }
        foreach (SpawnInstance spawn in wave.group2)
        {
            res += spawn.enemyData.coinReward * spawn.quantity;
        }
        foreach (SpawnInstance spawn in wave.group3)
        {
            res += spawn.enemyData.coinReward * spawn.quantity;
        }
        foreach (SpawnInstance spawn in wave.group4)
        {
            res += spawn.enemyData.coinReward * spawn.quantity;
        }
        return res;
    }
}