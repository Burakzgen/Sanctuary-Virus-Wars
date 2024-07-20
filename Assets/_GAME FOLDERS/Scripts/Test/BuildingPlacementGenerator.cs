using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementGenerator : MonoBehaviour
{

    public GameObject[] buildingPrefabs; // Yerle�tirilecek binalar�n prefablar�
    public int numberOfBuildings = 10; // Yerle�tirilecek bina say�s�
    public Terrain terrain; // Binalar�n yerle�tirilece�i terrain
    public float minDistanceBetweenBuildings = 10f; // Binalar aras�ndaki minimum mesafe
    public bool useRoads = false; // Yol noktalar�n� kullanma durumu
    public List<Vector3> roadPoints = new List<Vector3>(); // Yol noktalar�
    public Transform parentObject; // Parent obje

    private List<Vector3> placedBuildings = new List<Vector3>();

    void Start()
    {
        PlaceBuildings();
    }

    void PlaceBuildings()
    {
        for (int i = 0; i < numberOfBuildings; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            bool positionFound = false;

            // Uygun bir pozisyon bulana kadar d�ng�
            int attempts = 0;
            while (!positionFound && attempts < 100)
            {
                attempts++;
                if (useRoads && roadPoints.Count > 0)
                {
                    // Yol noktalar�na yak�n bir pozisyon se�
                    Vector3 pathPosition = GetRandomPositionAlongPath();
                    randomPosition = new Vector3(
                        pathPosition.x + Random.Range(-10, 10),
                        0,
                        pathPosition.z + Random.Range(-10, 10)
                    );
                }
                else
                {
                    // Rastgele bir pozisyon belirle
                    float randomX = Random.Range(0, terrain.terrainData.size.x);
                    float randomZ = Random.Range(0, terrain.terrainData.size.z);
                    randomPosition = new Vector3(randomX, 0, randomZ);
                }

                // Y�ksekli�i ayarla
                randomPosition.y = terrain.SampleHeight(randomPosition);

                // �ak��ma kontrol�
                if (!IsPositionColliding(randomPosition))
                {
                    positionFound = true;
                }
            }

            // E�er uygun bir pozisyon bulunamad�ysa devam et
            if (!positionFound) continue;

            // Rastgele bir prefab se�
            GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            // Bina yerle�tir
            GameObject buildingInstance = Instantiate(buildingPrefab, randomPosition, Quaternion.identity);

            // Parent objeye ekle
            if (parentObject != null)
            {
                buildingInstance.transform.SetParent(parentObject);
            }

            placedBuildings.Add(randomPosition);
        }
    }

    bool IsPositionColliding(Vector3 position)
    {
        foreach (Vector3 placedPosition in placedBuildings)
        {
            if (Vector3.Distance(position, placedPosition) < minDistanceBetweenBuildings)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 GetRandomPositionAlongPath()
    {
        if (roadPoints.Count < 2)
        {
            // Yeterli yol noktas� yoksa, ilk noktay� d�nd�r
            return roadPoints.Count == 1 ? roadPoints[0] : Vector3.zero;
        }

        // Rastgele iki nokta se�
        int index = Random.Range(0, roadPoints.Count - 1);
        Vector3 startPoint = roadPoints[index];
        Vector3 endPoint = roadPoints[index + 1];

        // �ki nokta aras�nda rastgele bir pozisyon se�
        float t = Random.Range(0f, 1f);
        return Vector3.Lerp(startPoint, endPoint, t);
    }

    // Editor'de yol �izgisini �izmek i�in OnDrawGizmos kullan
    void OnDrawGizmos()
    {
        if (roadPoints.Count > 1)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < roadPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(roadPoints[i], roadPoints[i + 1]);
            }
        }
    }
}
