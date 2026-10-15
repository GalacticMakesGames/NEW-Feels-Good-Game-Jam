using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;
    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    [SerializeField] private int maxEnemyLimit = 5;
    [SerializeField] private int resumeEnemySpawnThreshold = 2;

    private bool isSpawningRestricted = false;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        int currentEnemyCount = EnemyCountTracker.activeEnemyInstances.Count;

        if (isSpawningRestricted == false && currentEnemyCount >= maxEnemyLimit)
        {
            isSpawningRestricted = true;
        }
        else if (isSpawningRestricted == true && currentEnemyCount <= resumeEnemySpawnThreshold)
        {
            isSpawningRestricted = false;
        }

        timeUntilSpawn -= Time.deltaTime;

        if (isSpawningRestricted == false && timeUntilSpawn <= 0)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

            if (enemy != null )
            {
                // Tells the manager to get a random key for this specific enemy
                EnemyKeyManager.Instance.RegisterAndAssignKey(enemy);
            }

            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
