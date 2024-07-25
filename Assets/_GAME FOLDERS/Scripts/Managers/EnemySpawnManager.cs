using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : SingleReference<EnemySpawnManager>
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> attackerSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> stableSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> poisonerSpawnPoints = new List<Transform>();

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject attackerPrefab;
    [SerializeField] private GameObject stablePrefab;
    [SerializeField] private GameObject poisonerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private int initialAttackerCount = 30;
    [SerializeField] private int initialStableCount = 30;
    [SerializeField] private int initialPoisonerCount = 30;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private float spawnRadiusCheck = 2f;
    WaitForSeconds respawnEnemyDuration;

    private Dictionary<EnemyType, List<Transform>> spawnPoints;
    private Dictionary<EnemyType, GameObject> enemyPrefabs;

    public override void Awake()
    {
        respawnEnemyDuration = new WaitForSeconds(3f);
        spawnPoints = new Dictionary<EnemyType, List<Transform>>
        {
            { EnemyType.Attacker, attackerSpawnPoints },
            { EnemyType.Stable, stableSpawnPoints },
            { EnemyType.Poisoner, poisonerSpawnPoints }
        };

        enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Attacker, attackerPrefab },
            { EnemyType.Stable, stablePrefab },
            { EnemyType.Poisoner, poisonerPrefab }
        };
    }
    private void Start()
    {
        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        SpawnEnemies(EnemyType.Attacker, initialAttackerCount);
        SpawnEnemies(EnemyType.Stable, initialStableCount);
        SpawnEnemies(EnemyType.Poisoner, initialPoisonerCount);
    }

    private void SpawnEnemies(EnemyType enemyType, int count)
    {
        var points = spawnPoints[enemyType];
        var prefab = enemyPrefabs[enemyType];
        for (int i = 0; i < count; i++)
        {
            var spawnPoint = GetAvailableSpawnPoint(points);
            if (spawnPoint != null)
            {
                var newEnemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, enemiesParent);
                newEnemy.GetComponent<EnemyController>().Init(enemyType);
            }
        }
    }

    public void RespawnEnemy(EnemyType enemyType)
    {
        StartCoroutine(RespawnEnemyAfterDelay(enemyType));
    }

    private IEnumerator RespawnEnemyAfterDelay(EnemyType enemyType)
    {
        yield return new WaitForSeconds(respawnDelay);
        var points = spawnPoints[enemyType];
        var prefab = enemyPrefabs[enemyType];
        var spawnPoint = GetAvailableSpawnPoint(points);
        if (spawnPoint != null)
        {
            var newEnemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, enemiesParent);
            newEnemy.GetComponent<EnemyController>().Init(enemyType);
        }
    }
    private Transform GetAvailableSpawnPoint(List<Transform> points)
    {
        foreach (var point in points)
        {
            if (IsPositionAvailable(point.position))
            {
                return point;
            }
        }
        return null; // Boþ pozisyon yoksa null dönecek
    }
    private bool IsPositionAvailable(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, spawnRadiusCheck);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                return false;
            }
        }
        return true;
    }
}

public enum EnemyType
{
    Attacker,
    Patroller,
    Stable,
    Poisoner
}
