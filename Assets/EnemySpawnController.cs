using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] float timeBetweenSpawns = 0.7f;
    float timer;
    

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns)
        {
            Instantiate(enemyPrefab);
            timer = 0;
        }
    }
}
