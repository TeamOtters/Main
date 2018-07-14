using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Phase2UI : MonoBehaviour
{
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    public Canvas m_Phase2;
    private bool m_phase2 = false;
    public GameObject m_phase2Prompt;


    public float[] m_rankScale;

    private Vector3 targetScale;

   // public GameObject NSprite;
    public GameObject HSprite;
   // public Button scoreBoard;




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


    public void Update()
    {
        if(m_phase2)
        {
            
            for (int i = 0; i < m_scoreManager.m_ranks.Count; i++)
            {
                ScaleByRank(i);

            }

            // Stop scoring at end of game
            if (GameController.Instance.goalLine.GetGameOverState() == true)
            {
                m_Phase2.gameObject.SetActive(false);
            }
        }



    }

    private void ScaleByRank(int i)
    {
        targetScale = new Vector3(m_rankScale[i], m_rankScale[i], 1);
        Text scaleText = m_scoreManager.m_scoreBoardText[(m_scoreManager.m_ranks[i].playerIndex) - 1];
        Transform scaleV = scaleText.transform.parent;
        scaleV.localScale = new Vector3(targetScale.x, targetScale.y, 1);

        if (i == 0)
        {
            HSprite.gameObject.SetActive(true);
            HSprite.transform.SetParent(scaleV);
            HSprite.transform.position = scaleV.transform.position;
            HSprite.transform.localScale = scaleV.transform.localScale;
        }
    }

    /*
    private IEnumerator RankDelay(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (index < (m_scoreManager.m_scoreBoardText.Length) - 1)
        {
            index++;
            if(index == (m_scoreManager.m_scoreBoardText.Length) -1)
            {
                m_phase2 = true;
            }
        }

    }
    */

    public void ActivateScoreboard(int index)
    {
        int myRankIndex = (m_scoreManager.m_ranks[index].playerIndex) -1;
        m_scoreManager.m_scoreBoardText[myRankIndex].transform.parent.gameObject.SetActive(true);
        //m_scoreManager.m_scoreBoardText[myRankIndex].transform.parent.localScale = new Vector3(m_rankScale[myRankIndex], m_rankScale[myRankIndex], 1);

        targetScale = new Vector3(m_rankScale[index], m_rankScale[index], 1);
        Text scaleText = m_scoreManager.m_scoreBoardText[(m_scoreManager.m_ranks[index].playerIndex) - 1];
        Transform scaleV = scaleText.transform.parent;
        scaleV.localScale = new Vector3(targetScale.x, targetScale.y, 1);

        if (index == 0)
        {
            HSprite.gameObject.SetActive(true);
            HSprite.transform.SetParent(scaleV);
            HSprite.transform.position = scaleV.transform.position;
            HSprite.transform.localScale = scaleV.transform.localScale;
        }

        if (index < (m_scoreManager.m_scoreBoardText.Length))
        {
            index++;
            if (index == (m_scoreManager.m_scoreBoardText.Length))
            {
                m_phase2 = true;
            }
        }
        

    }

}

