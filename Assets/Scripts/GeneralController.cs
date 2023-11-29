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
        skillScreen = Instantiate(skillScreenPrefab, skillCanvas.transform); //instantiate skillScreen as child of canvas
        skillScreen.transform.localScale = new UnityEngine.Vector3(1, 1, 1); //reset scale, otherwise inherits from parent
        PauseGame();

        //Get buttons
        damageButton = GameObject.FindGameObjectWithTag("DamageButton").GetComponent<Button>();
        firerateButton = GameObject.FindGameObjectWithTag("FirerateButton").GetComponent<Button>();
        maxHealthButton = GameObject.FindGameObjectWithTag("MaxHealthButton").GetComponent<Button>();

        //Set actions to buttons
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
        ButtonEnd();
    }

    public void IncreaseFirerate()
    {
        playerScript.TimeBetweenShots *= 0.8f;
        ButtonEnd();
    }

    public void IncreaseMaxHealth()
    {
        playerScript.maxHealth += 10;
        ButtonEnd();
    }

    void PossiblySpawnHealth()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenHealth)
        {
            int a = UnityEngine.Random.Range(0, spawnChance); //randomize if health will actually get spawned
            if (a == 0)
            {
                UnityEngine.Vector3 randomPos = new UnityEngine.Vector3 //Get random position
                (
                UnityEngine.Random.Range(-Camera.main.orthographicSize * Screen.width / Screen.height + 0.3f, Camera.main.orthographicSize * Screen.width / Screen.height - 0.3f),
                UnityEngine.Random.Range(-Camera.main.orthographicSize + 0.3f, Camera.main.orthographicSize - 0.3f),
                0
                );

                //Find the random positions nearest point on NavMesh and set that as spawnpoint
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
        Time.timeScale = 0; //because movement and timer uses Time this will pause all movement and timers
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void ButtonEnd()
    {
        skillCanvas.SetActive(false);
        ResumeGame();
        playerScript.CheckLevel();
    }
}
