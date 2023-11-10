using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float maxHealth;
    float currentHealth;
    int maxSpeed = 5;


    [SerializeField] float xpValue;

    [SerializeField] Slider healthBar;
    [SerializeField] GameObject healthBarObject;

    GameObject player;
    PlayerController playerScript;

    NavMeshAgent agent;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        //Increase stats depending on player level
        for (int i = 0; i < playerScript.level; i++)
        {
            speed *= 1.05f;
            damage *= 1.05f;
            maxHealth *= 1.1f;
            xpValue *= 1.1f;
        }

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }

        currentHealth = maxHealth;
        healthBarObject.SetActive(false);

        //NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; //stay 2D
        agent.updateUpAxis = false; //stay 2D
        agent.speed = speed;
    }

    void Update()
    {
        // NavMesh movement
        agent.SetDestination(new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z));
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
                Killed();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerScript.currentHealth -= damage;
            Killed();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Killed()
    {
        playerScript.kills++;
        playerScript.currentXP += xpValue;
        GameObject.Destroy(this.gameObject);
    }
}
