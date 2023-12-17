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
    public Vector3 oscillation = new Vector3(2.0f, 1.5f, 0.0f);

    public float spawnNumberIncrease = 1.2f;
    public float spawnIntervalDecrease = 0.9f;

    bool _shouldSpawn = false;
    public bool shouldSpawn { 
        get => _shouldSpawn;
        set {
            _shouldSpawn = value;
            spawnTimer = 0f;
            regenerateTimer = 0f;
        }
    }

    float spawnTimer = 0f;
    float regenerateTimer = 0f;

    Vector3 basePosition;

    void Awake()
    {
        GenerateSpawnPoints();
        basePosition = transform.position;
    }

    void Start(){
        GameMgr.instance.OnGameStateChange += OnGameStateChange;
    }

    void OnGameStateChange(GameMgr.GameState oldState, GameMgr.GameState newState)
    {
        if (newState == GameMgr.GameState.Start)
        {
            shouldSpawn = false;
        }
        else if (newState == GameMgr.GameState.Playing)
        {
            shouldSpawn = true;
        }
        else if (newState == GameMgr.GameState.GameOver)
        {
            shouldSpawn = true;
            spawnInterval *= spawnIntervalDecrease;
            maxSimultaneous = (int)(maxSimultaneous * spawnNumberIncrease);
        }
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

        GameObject prefabToSpawn = prefabs[GameMgr.instance.WaveCnt % prefabs.Length];
        Instantiate(prefabToSpawn, transform.TransformPoint(point), Quaternion.identity);
    }

    void Update()
    {
        if (!shouldSpawn) return;

        transform.position = basePosition + new Vector3(
            Mathf.Lerp(-oscillation.x, oscillation.x, Mathf.PingPong(Time.time, 1)),
            Mathf.Lerp(-oscillation.y, oscillation.y, Mathf.PingPong(Time.time, 1)),
            Mathf.Lerp(-oscillation.z, oscillation.z, Mathf.PingPong(Time.time, 1))
        );

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
