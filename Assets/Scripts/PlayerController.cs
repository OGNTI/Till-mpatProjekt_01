using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;

    [SerializeField] Transform gunBarrelPos;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Slider healthBar;

    [SerializeField] GameObject eventThing;
    EnemySpawnController spawnScript;

    HealthBallController healthScript;

    GeneralController generalScript;

    public float timeBetweenShots = 1;
    float shotTimer;

    public float maxHealth = 100;
    float _currentHealth = 0;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            if (_currentHealth < 0) _currentHealth = 0;
            if (_currentHealth > maxHealth) _currentHealth = maxHealth;

        }
    }

    public int kills = 0;
    public int level = 0;
    float requiredXP = 1;
    public float currentXP = 0;
    public int damage = 20;

    Scene currentScene;

    static int savedKills;
    static int savedLevel;
    static float savedRequiredXP;
    static float savedCurrentXP;
    static int savedDamage;
    static float savedFirerate;
    static float savedMaxHealth;
    static float savedCurrentHealth;



    void Awake()
    {
        spawnScript = eventThing.GetComponent<EnemySpawnController>();
        generalScript = eventThing.GetComponent<GeneralController>();

        generalScript.ResumeGame();

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Main")
        {
            CurrentHealth = maxHealth;
        }
        else if (currentScene.name == "Map2")
        {
            LoadStats();
        }
    }

    void Update()
    {

        float walkSpeed = speed * Time.deltaTime;
        float rotationSpeed = speed * 2 * Time.deltaTime;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0).normalized * walkSpeed;
        transform.position += movement; //Translate does local position, this does global

        if (transform.position.y >= Camera.main.orthographicSize)
        {
            transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize, 0);
        }
        else if (transform.position.y <= -Camera.main.orthographicSize)
        {
            transform.position = new Vector3(transform.position.x, -Camera.main.orthographicSize, 0);
        }
        if (transform.position.x >= Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position = new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, transform.position.y, 0);
        }
        else if (transform.position.x <= -Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position = new Vector3(-Camera.main.orthographicSize * Screen.width / Screen.height, transform.position.y, 0);
        }


        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);

        shotTimer += Time.deltaTime;

        if (Input.GetAxisRaw("Fire1") > 0 && shotTimer > timeBetweenShots)
        {
            Instantiate(bulletPrefab, gunBarrelPos.position, transform.rotation);
            shotTimer = 0;
        }

        if (currentXP >= requiredXP)
        {
            LevelUp();
        }

        UpdateHealthBar();
        CheckLevel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HealthBall")
        {
            healthScript = other.GetComponent<HealthBallController>();
            CurrentHealth += healthScript.healthValue;
            GameObject.Destroy(other.gameObject);
        }
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= requiredXP;
        requiredXP *= 1.4f;
        spawnScript.timeBetweenSpawns *= 0.95f; 
        if (level == 1)
        {
            generalScript.CreateSkillScreen();
        }
        else
        {
            generalScript.ShowSkillScreen();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("End");
    }

    void CheckLevel()
    {
        if (currentScene.name == "Main" && level == 10)
        {
            SaveStats();
            SceneManager.LoadScene("Map2");
        }
    }

    void SaveStats()
    {
        savedKills = kills;
        savedLevel = level;
        savedRequiredXP = requiredXP;
        savedCurrentXP = currentXP;
        savedDamage = damage;
        savedFirerate = timeBetweenShots;
        savedMaxHealth = maxHealth;
        savedCurrentHealth = CurrentHealth;
    }

    void LoadStats()
    {
        kills = savedKills;
        level = savedLevel;
        requiredXP = savedRequiredXP;
        currentXP = savedCurrentXP;
        damage = savedDamage;
        timeBetweenShots = savedFirerate;
        maxHealth = savedMaxHealth;
        CurrentHealth = savedCurrentHealth;
    }
}
