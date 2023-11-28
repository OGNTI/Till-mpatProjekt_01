using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnController : MonoBehaviour
{
    public float timeBetweenSpawns;
    float timer;

    int bigEnemyChance = 1;

    [SerializeField] GameObject player;
    PlayerController playerScript;

    [SerializeField] GameObject regEnemyPrefab;
    [SerializeField] GameObject bigEnemyPrefab;
    GameObject enemyPrefab;

    Vector3[] spawns = { new Vector3(11.2f, 5, 0), new Vector3(11.2f, -5, 0), new Vector3(-11.2f, 5, 0), new Vector3(-11.2f, -5, 0) };
    Vector3 location;

    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Map2")
        {
            bigEnemyChance += 3;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns)
        {
            int a = UnityEngine.Random.Range(0, 10);
            if (a <= bigEnemyChance && playerScript.level >= 3)
            {
                enemyPrefab = bigEnemyPrefab;
            }
            else
            {
                enemyPrefab = regEnemyPrefab;
            }

            int b = UnityEngine.Random.Range(0, spawns.Length);
            location = spawns[b];
            if (Vector3.Distance(location, player.transform.position) <= 7) //If too close to spawn location, don't spawn
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
