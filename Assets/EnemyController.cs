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

    // [SerializeField] Slider healthBar;
    // [SerializeField] GameObject HealthBarObject;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currrentHealth = maxHealth;
        // HealthBarObject.SetActive(false);
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
                // healthBar = gameObject.AddComponent<Slider>();
                // HealthBarObject.SetActive(true);
            }
            currrentHealth -= 35;
            // UpdateHealthBar();

            if (currrentHealth <= 0)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

//     private void UpdateHealthBar()
//     {
//         healthBar.maxValue = maxHealth;
//         healthBar.value = currrentHealth;
//     }
}
