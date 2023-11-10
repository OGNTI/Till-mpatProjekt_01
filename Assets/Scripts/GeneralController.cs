using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GeneralController : MonoBehaviour
{
    [SerializeField] float timeBetweenHealth = 4;
    float timer;
    int spawnChance = 2; 
    [SerializeField] GameObject healthPrefab;

    [SerializeField] TMP_Text killsDisplay;
    string killsAmount;
    [SerializeField] TMP_Text levelDisplay;
    string levelValue;
    GameObject player;
    PlayerController playerScript;

    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        UpdateText();
        PossiblySpawnHealth();
    }

    void UpdateText()
    {
        killsAmount = playerScript.kills.ToString();
        killsDisplay.text = "Kills: " + killsAmount;

        levelValue = playerScript.level.ToString();
        levelDisplay.text = "Level: " + levelValue;
    }

    public void DamageButton()
    {
        playerScript.Damage += 5;
        playerScript.SkillIncreaseScreen(false);
        playerScript.ResumeGame();
    }

    public void FirerateButton()
    {
        playerScript.timeBetweenShots -= 0.2f;
        playerScript.SkillIncreaseScreen(false);

        if (playerScript.timeBetweenShots <= 0.1f)
        {
            playerScript.timeBetweenShots = 0.1f;
        }
        playerScript.ResumeGame();
    }

    public void MaxhealthButton()
    {
        playerScript.maxHealth += 10;
        playerScript.SkillIncreaseScreen(false);
        playerScript.ResumeGame();
    }

    void PossiblySpawnHealth()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenHealth)
        {
            int a = UnityEngine.Random.Range(0, spawnChance);
            if (a == 0)
            {
                UnityEngine.Vector3 randomPos = 
                new UnityEngine.Vector3
                (
                UnityEngine.Random.Range(-Camera.main.orthographicSize * Screen.width / Screen.height +0.3f, Camera.main.orthographicSize * Screen.width / Screen.height -0.3f), 
                UnityEngine.Random.Range(-Camera.main.orthographicSize +0.3f, Camera.main.orthographicSize -0.3f), 
                0
                );

                NavMesh.SamplePosition(randomPos, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
                UnityEngine.Vector3 randomPosInNavMesh = hit.position;

                Instantiate(healthPrefab, randomPosInNavMesh, quaternion.identity);
                timer = 0;
            }
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Main");
    }
}
