using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{


    public void onClick()


    { 
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu_Screen"))
        { 
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }

        else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Menu_Screen"))
        {
            SceneManager.LoadScene("Menu_Screen", LoadSceneMode.Single);
        }
    }


}
