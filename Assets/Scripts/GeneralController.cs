using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneralController : MonoBehaviour
{
    public TMP_Text killsDisplay;
    string killsAmount;
    public TMP_Text levelDisplay;
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
}
