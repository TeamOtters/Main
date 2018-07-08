using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

    }

    public void onClick()


    { 
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu_Screen"))
        { 
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainGame"))
        {
            SceneManager.LoadScene("Menu_Screen", LoadSceneMode.Single);
        }
    }


}
