using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawning : MonoBehaviour
{
    public GameObject enemyObject;

    public Transform homeBase;

    public int waveNumber = 1;
    public int enemyType = 1;


    private int enemiesPerWave = 3;  // Number of enemies per wave
    private float timeBetweenWaves = 10; // Time between waves in seconds
    private float moveSpeed = 5f; // Speed at which the enemy moves towards the center
    private Vector2 spawnRange = new Vector2(4f, 4f); // Range for random X, Y spawn positions

    private Vector3 targetPosition = Vector3.zero; // target position


    private int health;
    private int speed;
    private int damage;

    public List<Transform> spawnPoints;



    // Start is called before the first frame update
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
                SpawnEnemy(enemyType);
            }

            // Increase the number of enemies per wave and the move speed based on the difficulty level
            enemiesPerWave++;
            moveSpeed += 0.5f;

            // Wait for the specified time before spawning the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy(int enemyNumber)
    {
        // Randomly spawn the enemy at a random X, Y position within the defined range
        int spawnVal = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[spawnVal];

        Vector3 spawnPosition = spawnPoint.position;

        // Instantiate the enemy prefab at the spawn position
        GameObject enemy = Instantiate(enemyObject, spawnPosition, Quaternion.identity);
        enemy.transform.localScale = new Vector3(5f, 5f, 5f);
        // EnemyMover mover = enemy.GetComponent<EnemyMover>();
        // mover.moveSpeed = moveSpeed;

        if(enemyNumber == 1){
            health = 50;
            damage = 10;
            speed = 10;
        }
        else if(enemyNumber == 2){
            health = 100;
            damage = 50;
            speed = 8;
        }
        else if(enemyNumber == 3){
            health = 200;
            damage = 100;
            speed = 5;
        }
        else if(enemyNumber == 4){
            health = 1000;
            damage = 500;
            speed = 3;
        }

        enemy.GetComponent<EnemyAI>().homeBase = homeBase;
        enemy.GetComponent<EnemyHealth>().health = health;
        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed;
        enemy.GetComponent<EnemyAI>().damageAmount = damage;
    }
}