using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    public GameObject[] prefabToSpawn;
    public Transform spawnPoint;
    public float spawnInterval = 0.4f;
    public float despawnHeight = -5f;
    List<GameObject> spawnedObjects = new List<GameObject>();

    private float spawnTimer = 0f;
    public bool canSpawn = true;

    private void Start() {
        prefabToSpawn = new GameObject[]{
            Resources.Load<GameObject>("Prefabs/AppleFalling"),
            Resources.Load<GameObject>("Prefabs/Bananas"),
            Resources.Load<GameObject>("Prefabs/Kiwi"),
            Resources.Load<GameObject>("Prefabs/CherryFalling")
        };
    }

    private void Update()
    {
        if (canSpawn) {
            // Increment the spawn timer
            spawnTimer += Time.deltaTime;

            // Check if it's time to spawn a new prefab
            if (spawnTimer >= spawnInterval)
            {
                SpawnPrefab();
                spawnTimer = 0f; // Reset the spawn timer
            }
        }
        DespawnObjects();
    }

    private void SpawnPrefab()
    {
         Vector3 randomOffset = new Vector3(
            Random.Range(-7f, 7f),
            0f,
            0f
        );

        Vector3 spawnPosition = spawnPoint.position + randomOffset;
        GameObject objectS = prefabToSpawn[Random.Range(0,4)];
        spawnedObjects.Add(objectS);
        // Instantiate the prefab at the spawn point position and rotation
        Instantiate(objectS, spawnPosition, spawnPoint.rotation);
    }

    private void DespawnObjects() {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj.transform.position.y <= despawnHeight)
            {
                Destroy(obj);
            }
        }
    }
}
