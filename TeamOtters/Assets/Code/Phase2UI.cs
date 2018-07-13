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
    private int m_playerRank;
    public GameObject m_phase2Prompt;
    private int pIndex;
    private bool isInCoroutine = false;


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
        for (int i = 0; i < m_scoreManager.m_ranks.Count; i++)
        {
            Debug.Log(i);
            targetScale = new Vector3(m_rankScale[i], m_rankScale[i], 1);
            Text scaleText = m_scoreManager.scoreText[(m_scoreManager.m_ranks[i].playerIndex) - 1];
            Transform scaleV = scaleText.transform.parent;
            scaleV.localScale = new Vector3(scaleV.localScale.x * targetScale.x, scaleV.localScale.y * targetScale.y, 1);

            if (i==0)
            {
                HSprite.transform.parent = scaleV;
                HSprite.transform.position = scaleV.transform.position;
                HSprite.transform.localScale = scaleV.transform.localScale; 
            }


        }

       
    }


    private IEnumerator RankDelay(int index)
    {
        yield return new WaitForSeconds(0.5f);
        if (index < (m_scoreManager.scoreText.Length) - 1)
        {
            index++;
            ActivateScoreboard(index);
        }

    }

    public void ActivateScoreboard(int index)
    {
        m_scoreManager.scoreText[(m_scoreManager.m_ranks[index].playerIndex) - 1].transform.parent.gameObject.SetActive(true);
        StartCoroutine(RankDelay(index));
    }

    /*public void ChangeImage()
    {
        if (scoreBoard.image.sprite == NSprite)
            scoreBoard.image.sprite = HSprite;
        else
        {
            scoreBoard.image.sprite = NSprite;
        }

    }*/
}

