using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RandomPrefabSpawner))]
public class RandomPrefabSpawnerEditor : Editor
{
    void OnSceneGUI()
    {
        RandomPrefabSpawner spawner = (RandomPrefabSpawner)target;
        // draw plane
        Handles.color = Color.blue;
        Handles.DrawWireCube(spawner.transform.position, new Vector3(spawner.planeSize, 0, spawner.planeSize));

        if (spawner.spawnPoints == null) return;

        Handles.color = Color.red;
        foreach (Vector3 point in spawner.spawnPoints)
        {
            var worldPoint = spawner.transform.TransformPoint(point); // Transform local point to world space
            Handles.DrawWireCube(worldPoint, Vector3.one); // Draw a cube at each spawn point
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomPrefabSpawner spawner = (RandomPrefabSpawner)target;
        if (GUILayout.Button("Generate Spawn Points"))
        {
            spawner.GenerateSpawnPoints();
            SceneView.RepaintAll(); // Update the scene view to show new points
        }
    }
}
