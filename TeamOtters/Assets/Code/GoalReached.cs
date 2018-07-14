using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class GoalReached : MonoBehaviour {
    public Canvas m_Results;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    private int m_carryingScoreBonus;
    private int m_normalScoreBonus;
    private bool m_hasReachedValhalla=false;
    public Button m_restart;
    private Phase2UI m_phase2canvas; 

    // Use this for initialization
    void Start ()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
        /*
        m_normalScoreBonus = m_gameController.firstReachGoal;
        m_carryingScoreBonus = m_gameController.carryingBonus;*/
	}

    public bool GetGameOverState()
    {        
        return m_hasReachedValhalla;
    }

    private void OnTriggerEnter(Collider player)
    {
        int myOldScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
        int myNewScore = 0;

        if(m_hasReachedValhalla)
        {
            GameController.Instance.cameraManager.m_shouldSidewaysPan = false;
        }
        if (!m_hasReachedValhalla)
        {
            if (player.gameObject.CompareTag("Valkyrie") && player.gameObject.GetComponent<ValkyrieController>().isCarrying == (true))
            {
                m_hasReachedValhalla = true;
                int ID = player.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex;
                m_scoreManager.AddToScore(ScorePointInfo.carryingBonus, ID);

                StartCoroutine("Wait");
                myNewScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
            }
            else if (player.gameObject.CompareTag("Viking"))
            {
                if(player.GetComponent<VikingRespawn>().m_hasRespawned)
                {

                }
                else
                {
                    m_hasReachedValhalla = true;
                    int ID = player.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex;
                    m_scoreManager.AddToScore(ScorePointInfo.firstReachGoal, ID);
                    myNewScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
                    StartCoroutine("Wait");
                }


            }
        }
        Debug.Log("I crossed the finish line, my old score was " + myOldScore + "and my new score was " + myNewScore);


    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        m_Results.gameObject.SetActive(true);
        m_Results.GetComponentInChildren<Results>().ShowResults();
        m_restart.Select();
        m_phase2canvas.gameObject.SetActive(false); 
        
        

    }
    // Update is called once per frame
    void Update () {
		
	}
}
