using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralController : MonoBehaviour
{
    [SerializeField] float timeBetweenHealth = 4;
    float timer;
    int spawnChance = 2;
    [SerializeField] GameObject healthPrefab;

    [SerializeField] GameObject skillScreenPrefab;
    GameObject skillScreen;
    GameObject skillCanvas;
    Button Damage;
    Button Firerate;
    Button MaxHealth;

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

        skillCanvas = GameObject.FindGameObjectWithTag("SkillCanvas");
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

    public void CreateSkillScreen()
    {
        skillScreen = Instantiate(skillScreenPrefab, skillCanvas.transform);
        skillScreen.transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        PauseGame();

        GameObject DB = GameObject.FindGameObjectWithTag("DamageButton");
        GameObject FB = GameObject.FindGameObjectWithTag("FirerateButton");
        GameObject HB = GameObject.FindGameObjectWithTag("MaxHealthButton");

        //set damage, firerate to the component button in ^^
    }

    public void ShowSkillScreen()
    {
        skillCanvas.SetActive(true);
        PauseGame();
    }

    public void DamageButton()
    {
        playerScript.Damage += 5;
        skillCanvas.SetActive(false);
        ResumeGame();
    }

    public void FirerateButton()
    {
        playerScript.timeBetweenShots -= 0.2f;
        if (playerScript.timeBetweenShots <= 0.1f)
        {
            playerScript.timeBetweenShots = 0.1f;
        }
        skillCanvas.SetActive(false);
        ResumeGame();
    }

    public void MaxHealthButton()
    {
        playerScript.maxHealth += 10;
        skillCanvas.SetActive(false);
        ResumeGame();
    }

    void PossiblySpawnHealth()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenHealth)
        {
            int a = UnityEngine.Random.Range(0, spawnChance);
            if (a == 0)
            {
                UnityEngine.Vector3 randomPos =
                new UnityEngine.Vector3
                (
                UnityEngine.Random.Range(-Camera.main.orthographicSize * Screen.width / Screen.height + 0.3f, Camera.main.orthographicSize * Screen.width / Screen.height - 0.3f),
                UnityEngine.Random.Range(-Camera.main.orthographicSize + 0.3f, Camera.main.orthographicSize - 0.3f),
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

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
