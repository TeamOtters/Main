using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Phase2UI: MonoBehaviour {

    public GameObject m_PullGamePhase;
    public Canvas m_Phase2;
    public Canvas m_InGameMenu;
    private bool m_Phase2Active=false;

    // Use this for initialization
    void Start () {
        m_Phase2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!m_Phase2Active) { 
        GameObject valkyrie = GameObject.FindGameObjectWithTag("Valkyrie");


        if (valkyrie)
        {
            Debug.Log("Valkyrie Found");
            m_Phase2.gameObject.SetActive(true);
                m_Phase2Active = true;

        }
        }
    }

}

