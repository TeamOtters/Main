using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LoadSequence : MonoBehaviour {

    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    public Canvas m_Phase2;
    private bool m_phase2 = false;
    private int m_playerRank;
    public GameObject m_phase2Prompt; 



    // Use this for initialization
    public void Start()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
    }

    public void Update()
    {
        if (!m_Phase2)
        {
            m_phase2Prompt.gameObject.SetActive(true);
            StartCoroutine("LoadDelay");
            
           
        }
    }

    private IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(1.0f);
        m_phase2Prompt.gameObject.SetActive(false);

    }

    private IEnumerator RankDelay()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
