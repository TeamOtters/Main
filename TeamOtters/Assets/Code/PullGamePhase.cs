using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PullGamePhase : MonoBehaviour {

    public GameObject m_PullGamePhase;
    public Canvas m_Phase2;
    public Canvas m_InGameMenu;

    // Use this for initialization
    void Start () {
        m_PullGamePhase.gameObject.SetActive(true);
        m_Phase2.gameObject.SetActive(false);
    }

    void Update()
    {
        GameObject valkyrie = GameObject.FindGameObjectWithTag("Valkyrie");

        if (valkyrie)
        {
            Debug.Log("Valkyrie Found");
            m_Phase2.gameObject.SetActive(true);
            

        }

        if (m_InGameMenu.enabled)
        {
            m_Phase2.gameObject.SetActive(false);
        }
    }

}

