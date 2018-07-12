using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Phase2UI: MonoBehaviour {
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

    public void HidePrompt()
    {
        m_phase2Prompt.gameObject.SetActive(false);
    }



    private IEnumerator RankDelay(int index)
    {
        yield return new WaitForSeconds(0.8f);
        if (index < (m_scoreManager.scoreText.Length)-1)
        {
            index++;
            ActivateScoreboard(index);
        }

    }

    public void ActivateScoreboard(int index)
    {
        m_scoreManager.scoreText[(m_scoreManager.m_ranks[index].playerIndex)-1].transform.parent.gameObject.SetActive(true);
        StartCoroutine(RankDelay(index));
    }

}

