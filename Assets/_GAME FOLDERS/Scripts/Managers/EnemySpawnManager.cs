using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> attackerSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> patrollerSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> stableSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> poisonerSpawnPoints = new List<Transform>();

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject attackerPrefab;
    [SerializeField] private GameObject patrollerPrefab;
    [SerializeField] private GameObject stablePrefab;
    [SerializeField] private GameObject poisonerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxAttackerCount = 5;
    [SerializeField] private int maxPatrollerCount = 5;
    [SerializeField] private int maxStableCount = 5;
    [SerializeField] private int maxPoisonerCount = 5;

    private Dictionary<EnemyType, int> enemyCount;
    private Dictionary<EnemyType, int> maxEnemyCount;

    private void Start()
    {
        InitializeEnemyCounts();
        InvokeRepeating("CheckAndSpawnEnemies", spawnInterval, spawnInterval);
    }

    private void InitializeEnemyCounts()
    {
        enemyCount = new Dictionary<EnemyType, int>
        {
            { EnemyType.Attacker, 0 },
            { EnemyType.Patroller, 0 },
            { EnemyType.Stable, 0 },
            { EnemyType.Poisoner, 0 }
        };

        maxEnemyCount = new Dictionary<EnemyType, int>
        {
            { EnemyType.Attacker, maxAttackerCount },
            { EnemyType.Patroller, maxPatrollerCount },
            { EnemyType.Stable, maxStableCount },
            { EnemyType.Poisoner, maxPoisonerCount }
        };
    }

    private void CheckAndSpawnEnemies()
    {
        CheckAndSpawnEnemyType(attackerSpawnPoints, EnemyType.Attacker, attackerPrefab);
        CheckAndSpawnEnemyType(patrollerSpawnPoints, EnemyType.Patroller, patrollerPrefab);
        CheckAndSpawnEnemyType(stableSpawnPoints, EnemyType.Stable, stablePrefab);
        CheckAndSpawnEnemyType(poisonerSpawnPoints, EnemyType.Poisoner, poisonerPrefab);
    }

    private void CheckAndSpawnEnemyType(List<Transform> spawnPoints, EnemyType enemyType, GameObject prefab)
    {
        if (enemyCount[enemyType] < maxEnemyCount[enemyType])
        {
            foreach (var spawnPoint in spawnPoints)
            {
                if (enemyCount[enemyType] >= maxEnemyCount[enemyType])
                    break;

                SpawnEnemy(spawnPoint, enemyType, prefab);
            }
        }
    }

    private void SpawnEnemy(Transform spawnPoint, EnemyType enemyType, GameObject prefab)
    {
        GameObject newEnemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        var enemyController = newEnemy.GetComponent<EnemyController>();
        enemyController.OnEnemyKilled += () => OnEnemyKilled(enemyType);
        enemyCount[enemyType]++;
    }

    private void OnEnemyKilled(EnemyType enemyType)
    {
        enemyCount[enemyType]--;
    }
}

public enum EnemyType
{
    Attacker,
    Patroller,
    Stable,
    Poisoner
}
