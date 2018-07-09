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
            SceneManager.LoadScene("Phase2", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Phase1");
            
        }
    }

}

