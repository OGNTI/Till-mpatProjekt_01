using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [SerializeField] int xpValue = 1;

    [SerializeField] Slider healthBar;
    [SerializeField] GameObject healthBarObject;

    GameObject player;
    PlayerController playerScript;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        currentHealth = maxHealth;
        healthBarObject.SetActive(false);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (currentHealth == maxHealth)
            {
                healthBarObject.SetActive(true);
            }
            currentHealth -= playerScript.Damage;
            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                playerScript.kills++;
                playerScript.currentXP += xpValue;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
