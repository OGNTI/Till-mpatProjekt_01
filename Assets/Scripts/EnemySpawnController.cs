using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] float timeBetweenSpawns = 0.7f;
    float timer;

    [SerializeField] Transform playerPos;

    [SerializeField] GameObject regEnemyPrefab;
    [SerializeField] GameObject bigEnemyPrefab;
    GameObject enemyPrefab;

    Vector3[] spawns = {new Vector3(11.2f, 5, 0), new Vector3(11.2f, -5, 0), new Vector3(-11.2f, 5, 0), new Vector3(-11.2f, -5, 0)};
    Vector3 location;


    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns)
        {
            int a = UnityEngine.Random.Range(0, 4);
            if (a == 0)
            {
                enemyPrefab = bigEnemyPrefab;
            }
            else
            {
                enemyPrefab = regEnemyPrefab;
            }

            int b = UnityEngine.Random.Range(0, spawns.Length);
            location = spawns[b];
            if (Vector3.Distance(location, playerPos.position) <= 7) //If too close to spawn location, don't spawn
            {
                
            }
            else
            {
                Instantiate(enemyPrefab, location, quaternion.identity);
                timer = 0;
            }
        }
    }
}
