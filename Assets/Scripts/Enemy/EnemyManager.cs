using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject scientist, guard, jetpack;
    [SerializeField] Transform groundSpawnParent, airSpawnParent;
    float scientistSpawnThreshold, guardSpawnThreshold, spawnInterval;
    const int maxEnemyCount = 60;
    private void Awake()
    {
        scientistSpawnThreshold = 70;
        guardSpawnThreshold = 100;
        spawnInterval = 1;
        InvokeRepeating("SpawnEnemies", 2f, spawnInterval);
        InvokeRepeating("SpawnJetpacks", 5f, 3f);
    }

    private void SpawnEnemies()
    {
        if (transform.childCount < maxEnemyCount)
        {
            for (int i=0; i<groundSpawnParent.childCount; i++)
            {
                Transform t = groundSpawnParent.GetChild(i);
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

    private void SpawnJetpacks()
    {
        if (transform.childCount < maxEnemyCount)
        {
            for (int i = 0; i < airSpawnParent.childCount; i++)
            {
                Transform t = airSpawnParent.GetChild(i);
                Instantiate(jetpack, t.position, Quaternion.identity, transform);
            }
        }
    }
}
