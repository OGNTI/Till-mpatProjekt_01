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
    Button damageButton;
    Button firerateButton;
    Button maxHealthButton;

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

        damageButton = GameObject.FindGameObjectWithTag("DamageButton").GetComponent<Button>();
        firerateButton = GameObject.FindGameObjectWithTag("FirerateButton").GetComponent<Button>();
        maxHealthButton = GameObject.FindGameObjectWithTag("MaxHealthButton").GetComponent<Button>();

        damageButton.onClick.AddListener(IncreaseDamage);
        firerateButton.onClick.AddListener(IncreaseFirerate);
        maxHealthButton.onClick.AddListener(IncreaseMaxHealth);
    }

    public void ShowSkillScreen()
    {
        skillCanvas.SetActive(true);
        PauseGame();
    }

    public void IncreaseDamage()
    {
        playerScript.damage += 5;
        skillCanvas.SetActive(false);
        ResumeGame();
    }

    public void IncreaseFirerate()
    {
        playerScript.timeBetweenShots *= 0.8f;
        if (playerScript.timeBetweenShots <= 0.01f)
        {
            playerScript.timeBetweenShots = 0.01f;
        }
        skillCanvas.SetActive(false);
        ResumeGame();
    }

    public void IncreaseMaxHealth()
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
