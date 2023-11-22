using UnityEngine;

using System.Collections.Generic;
using System.Linq;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float planeSize = 10f;
    public List<Vector3> spawnPoints = new List<Vector3>();
    public int numberOfPoints = 10;
    public float minSeparation = 2f;
    public float spawnInterval = 1f;
    public int maxSimultaneous = 5;
    float spawnTimer = 0f;
    float regenerateTimer = 0f;

    void Awake()
    {
        GenerateSpawnPoints();
    }

    public void GenerateSpawnPoints()
    {
        spawnPoints.Clear();
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 point;
            do {
                point = new Vector3(
                    Random.Range(-planeSize / 2, planeSize / 2),
                    0,
                    Random.Range(-planeSize / 2, planeSize / 2)
                );
            } while (spawnPoints.Exists(p => Vector3.Distance(p, point) < minSeparation));
            
            spawnPoints.Add(point);
        }
    }

    public void SpawnRandomPrefabAtPoint(Vector3 point)
    {
        if (prefabs.Length == 0) return;

        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(prefabToSpawn, transform.TransformPoint(point), Quaternion.identity);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        regenerateTimer += Time.deltaTime;
        
        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0f;
            int numSpawned = Mathf.Min(Random.Range(1, maxSimultaneous), spawnPoints.Count);
            for (int i = 0; i < numSpawned; i++)
            {
                Vector3 point = spawnPoints[Random.Range(0, spawnPoints.Count)];
                SpawnRandomPrefabAtPoint(point);
            }
        }

        if (regenerateTimer > 5f)
        {
            regenerateTimer = 0f;
            GenerateSpawnPoints();
        }
    }
}
