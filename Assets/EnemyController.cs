using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] int maxHealth = 100;
    int currrentHealth;

    GameObject player;
    PlayerController playerScript;

    [SerializeField] Slider healthBar;
    [SerializeField] GameObject healthBarObject;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        currrentHealth = maxHealth;
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
            if (currrentHealth == maxHealth)
            {
                healthBarObject.SetActive(true);
            }
            currrentHealth -= 45;
            UpdateHealthBar();

            if (currrentHealth <= 0)
            {
                playerScript.kills++;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currrentHealth;
    }
}
