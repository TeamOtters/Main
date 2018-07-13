using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Animations;

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

    public GameObject HSprite;
    private bool scored=false;

    // Grow parameters
    public float approachSpeed = 0.02f;
    public float growthBound = 2f;
    public float shrinkBound = 0.5f;
    private float currentRatio = 1;

    // The object we're trying to manipulate
    private Button scoreBoard;

    // And something to do the manipulating
    private Coroutine routine;
    private bool keepGoing = true;
    private bool closeEnough = false;



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
            //Scoreboard Load Sequencing
            for (int i = 0; i < m_scoreManager.m_ranks.Count; i++)
            {
                targetScale = new Vector3(m_rankScale[i], m_rankScale[i], 1);
                Text scaleText = m_scoreManager.scoreText[(m_scoreManager.m_ranks[i].playerIndex) - 1];
                Transform scaleV = scaleText.transform.parent;
                scaleV.localScale = new Vector3(targetScale.x, targetScale.y, 1);
                
     

                if (i==0)
                {
                   
                    HSprite.transform.parent = scaleV;
                    HSprite.transform.position = scaleV.transform.position;
                    HSprite.transform.localScale = scaleV.transform.localScale;
                    HSprite.gameObject.SetActive(true); 
                }

                if (!scored)
                {
                    scoreBoard = scaleV.gameObject.GetComponent<Button>();
                    this.routine = StartCoroutine(this.Pulse());
                }
                

            }
        }
        
       
    }


    IEnumerator Pulse()
    {
        // Run this indefinitely
        while (keepGoing)
        {
            // Get bigger for a few seconds
            while (this.currentRatio != this.growthBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);

                // Update our text element
                this.gameObject.transform.localScale = Vector3.one * currentRatio;
                Debug.Log ("Growing!");

                yield return new WaitForEndOfFrame();
            }

            // Shrink for a few seconds
            while (this.currentRatio != this.shrinkBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);

                // Update our text element
                this.gameObject.transform.localScale = Vector3.one * currentRatio;
                Debug.Log("Shrinking!");

                yield return new WaitForEndOfFrame();
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
        m_phase2 = true;
    }

  
}

