using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PullGamePhase : MonoBehaviour {

    public Scene m_Active;
    public GameObject m_PullGamePhase;

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("Phase1", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Phase1"));

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Phase1"))
        {
            m_PullGamePhase.gameObject.SetActive(false);
            }

        }
	}

