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
    private int pIndex;
    private bool isInCoroutine = false;



    // Use this for initialization
    public void Start()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
    }

    

//Start Splitting LoadSequence 

    public void ShowPrompt()
    {
        m_phase2Prompt.gameObject.SetActive(true);
    }

    


    
    private IEnumerator RankDelay(int index)
    {
        yield return new WaitForSeconds(0.2f);
        if (index<m_scoreManager.m_scoreBoardText.Length)
        {
            index++;
            ActivateScoreboard(index);
        }

    }

    private void ActivateScoreboard(int index)
    {

        m_scoreManager.m_scoreBoardText[index].transform.parent.gameObject.SetActive(true);
        StartCoroutine(RankDelay(index));
        Debug.Log("Player" + index + "is loaded");
    }
}

/*
 *  public GameObject m_PullGamePhase;
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



*/