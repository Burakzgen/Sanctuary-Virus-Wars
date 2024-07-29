using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : SingleReference<InteractableManager>
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Interactable Prefabs")]
    [SerializeField] private List<GameObject> interactablePrefabs = new List<GameObject>();

    private Dictionary<Transform, GameObject> occupiedSpawnPoints;
    [SerializeField] Transform interactablesParent;

    [SerializeField] private float respawnDelay = 5f;

    protected override void Awake()
    {
        base.Awake();
        occupiedSpawnPoints = new Dictionary<Transform, GameObject>();
    }

    private void Start()
    {
        SpawnInitialInteractables();
    }

    private void SpawnInitialInteractables()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (!occupiedSpawnPoints.ContainsKey(spawnPoint))
            {
                GameObject prefab = GetRandomPrefab();
                if (prefab != null)
                {
                    SpawnInteractable(spawnPoint, prefab);
                }
            }
        }
    }

    private GameObject GetRandomPrefab()
    {
        if (interactablePrefabs.Count == 0)
            return null;

        return interactablePrefabs[Random.Range(0, interactablePrefabs.Count)];
    }

    private void SpawnInteractable(Transform spawnPoint, GameObject prefab)
    {
        GameObject newInteractable = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, interactablesParent);
        occupiedSpawnPoints[spawnPoint] = newInteractable;
        var interactable = newInteractable.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.OnCollected += () => OnInteractableCollected(spawnPoint, newInteractable);
        }
    }

    private void OnInteractableCollected(Transform spawnPoint, GameObject interactable)
    {
        occupiedSpawnPoints.Remove(spawnPoint);
        Destroy(interactable);
        StartCoroutine(RespawnInteractableAfterDelay(spawnPoint, respawnDelay));
    }

    private IEnumerator RespawnInteractableAfterDelay(Transform spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnInteractable(spawnPoint);
    }

    private void RespawnInteractable(Transform spawnPoint)
    {
        GameObject prefab = GetRandomPrefab();
        if (prefab != null)
        {
            SpawnInteractable(spawnPoint, prefab);
        }
    }
}
