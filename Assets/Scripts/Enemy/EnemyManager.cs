using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject scientist, guard, jetpack;
    [SerializeField] Transform groundSpawnParent, airSpawnParent;
    float scientistSpawnThreshold, guardSpawnThreshold, groundSpawnInterval, airSpawnInterval;
    const int maxEnemyCount = 60;
    private void Awake()
    {
        scientistSpawnThreshold = 70;
        guardSpawnThreshold = 100;
        groundSpawnInterval = 1;
        airSpawnInterval = 3;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnJetpacks());
        InvokeRepeating("ReduceSpawnIntervals", 30f, 30f);
    }

    private void ReduceSpawnIntervals()
    {
        if (groundSpawnInterval > 0.1f)
        {
            groundSpawnInterval -= 0.1f;
        }
        if (airSpawnInterval > 1.5f)
        {
            airSpawnInterval -= 0.5f;
        }
        if (scientistSpawnThreshold > 20)
        {
            scientistSpawnThreshold -= 10;
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (transform.childCount < maxEnemyCount)
            {
                for (int i = 0; i < groundSpawnParent.childCount; i++)
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
            yield return new WaitForSeconds(groundSpawnInterval);
        }
    }

    IEnumerator SpawnJetpacks()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (transform.childCount < maxEnemyCount)
            {
                for (int i = 0; i < airSpawnParent.childCount; i++)
                {
                    Transform t = airSpawnParent.GetChild(i);
                    Instantiate(jetpack, t.position, Quaternion.identity, transform);
                }
            }
            yield return new WaitForSeconds(airSpawnInterval);
        }
    }
}
