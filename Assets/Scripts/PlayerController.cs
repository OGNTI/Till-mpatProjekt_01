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

    GameObject skillScreen;

    public float timeBetweenShots = 1;
    float shotTimer;

    public float maxHealth = 100;
    public float currentHealth = 0;

    public int kills = 0;
    public int level = 0;
    float requiredXP = 2;
    public float currentXP = 0;
    public int Damage = 20;

    void Awake()
    {
        skillScreen = GameObject.FindGameObjectWithTag("SkillScreen");
        SkillIncreaseScreen(false);
        currentHealth = maxHealth;
    }

    void Update()
    {
        float walkSpeed = speed * Time.deltaTime;
        float rotationSpeed = speed * 2 * Time.deltaTime;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0).normalized * walkSpeed;

        if (transform.position.y >= Camera.main.orthographicSize || transform.position.y <= -Camera.main.orthographicSize)
        {
            movement.y *= -1;
        }
        else if (transform.position.x >= Camera.main.orthographicSize * Screen.width / Screen.height || transform.position.x <= -Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            movement.x *= -1;
        }

        transform.position += movement; //Translate does local position, this does global

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
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= requiredXP;
        requiredXP *= 1.4f;
        SkillIncreaseScreen(true);
    }

    public void SkillIncreaseScreen(bool gae)
    {
        skillScreen.SetActive(gae);
        if (gae == true)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void Die()
    {
        SceneManager.LoadScene("End");
    }
}