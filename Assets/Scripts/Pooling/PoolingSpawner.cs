using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSpawner : MonoBehaviour
{
    [SerializeField] private float timeToSpawn;
    [SerializeField] private GameObject objectsToSpawn;

    private float timeSinceSpawn;
    private PoolingMaster objectPool => DataManager.masterPool;

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn >= timeToSpawn)
        {
            GameObject newObject = objectPool.GetObject(objectsToSpawn);
            newObject.transform.position = transform.position;
            timeSinceSpawn = 0f;
        }
    }
}
