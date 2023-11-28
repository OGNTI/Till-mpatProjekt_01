using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBallController : MonoBehaviour
{
    public int healthValue;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Map2")
        {
            healthValue *= 2;
        }
    }
}
