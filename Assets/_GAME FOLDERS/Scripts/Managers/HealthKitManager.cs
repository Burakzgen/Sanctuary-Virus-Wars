using UnityEngine;

public class HealthKitManager : SingleReference<HealthKitManager>
{
    [SerializeField] private HealthKit healthKitPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void RespawnHealthKit(HealthKit healthKit)
    {
        Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        Destroy(healthKit.gameObject);
        Instantiate(healthKitPrefab, randomSpawnPoint, Quaternion.identity);
    }
}
