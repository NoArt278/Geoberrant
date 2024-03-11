using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject scientist, guard;
    [SerializeField] List<Transform> spawnPoints;
    float scientistSpawnThreshold, guardSpawnThreshold, spawnInterval;
    const int maxEnemyCount = 60;
    private void Awake()
    {
        scientistSpawnThreshold = 70;
        guardSpawnThreshold = 100;
        spawnInterval = 1;
        InvokeRepeating("SpawnEnemies", 2f, spawnInterval);
    }

    private void SpawnEnemies()
    {
        if (transform.childCount < maxEnemyCount)
        {
            foreach (Transform t in spawnPoints)
            {
                float res = Random.Range(0, 100);
                if (res <= scientistSpawnThreshold)
                {
                    Instantiate(scientist, t.position, Quaternion.identity, transform);
                }
                else if (res <= guardSpawnThreshold)
                {
                    Instantiate(guard, t.position, Quaternion.identity, transform);
                }
            }
        }
    }
}
