using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public TMP_Text killsDisplay;
    string MyText;
    PlayerController playerScript;
    GameObject player;

    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        MyText = playerScript.kills.ToString();
        killsDisplay.text = MyText;
    }
}
