using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementGenerator : MonoBehaviour
{

    public GameObject[] buildingPrefabs; // Yerleþtirilecek binalarýn prefablarý
    public int numberOfBuildings = 10; // Yerleþtirilecek bina sayýsý
    public Terrain terrain; // Binalarýn yerleþtirileceði terrain
    public float minDistanceBetweenBuildings = 10f; // Binalar arasýndaki minimum mesafe
    public bool useRoads = false; // Yol noktalarýný kullanma durumu
    public List<Vector3> roadPoints = new List<Vector3>(); // Yol noktalarý
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

            // Uygun bir pozisyon bulana kadar döngü
            int attempts = 0;
            while (!positionFound && attempts < 100)
            {
                attempts++;
                if (useRoads && roadPoints.Count > 0)
                {
                    // Yol noktalarýna yakýn bir pozisyon seç
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

                // Yüksekliði ayarla
                randomPosition.y = terrain.SampleHeight(randomPosition);

                // Çakýþma kontrolü
                if (!IsPositionColliding(randomPosition))
                {
                    positionFound = true;
                }
            }

            // Eðer uygun bir pozisyon bulunamadýysa devam et
            if (!positionFound) continue;

            // Rastgele bir prefab seç
            GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            // Bina yerleþtir
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
            // Yeterli yol noktasý yoksa, ilk noktayý döndür
            return roadPoints.Count == 1 ? roadPoints[0] : Vector3.zero;
        }

        // Rastgele iki nokta seç
        int index = Random.Range(0, roadPoints.Count - 1);
        Vector3 startPoint = roadPoints[index];
        Vector3 endPoint = roadPoints[index + 1];

        // Ýki nokta arasýnda rastgele bir pozisyon seç
        float t = Random.Range(0f, 1f);
        return Vector3.Lerp(startPoint, endPoint, t);
    }

    // Editor'de yol çizgisini çizmek için OnDrawGizmos kullan
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
