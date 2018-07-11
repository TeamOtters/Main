using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PullGamePhase : MonoBehaviour {


    public Scene m_Active;
    public GameObject m_PullGamePhase;
    public Canvas m_Phase2;
    public Canvas m_InGameMenu;

    // Use this for initialization
    void Start () {
        SceneManager.LoadScene("Phase1", LoadSceneMode.Additive);
	}

    void Update()
    {
        GameObject valkyrie = GameObject.FindGameObjectWithTag("Valkyrie");

        if (valkyrie)
        {
            Debug.Log("Valkyrie Found");
            m_PullGamePhase.gameObject.SetActive(false);
            m_Phase2.gameObject.SetActive(true);
            SceneManager.UnloadSceneAsync("Phase1");
            
        }

        if (m_InGameMenu.enabled)
        {
            m_Phase2.gameObject.SetActive(false);
        }
    }

}

