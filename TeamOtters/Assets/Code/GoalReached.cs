using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class GoalReached : MonoBehaviour {    
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    private int m_carryingScoreBonus;
    private int m_normalScoreBonus;
    public bool m_hasReachedValhalla;    
    public Canvas m_phase2canvas; 

    // Use this for initialization
    void Start ()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
        m_hasReachedValhalla = false;
        /*
        m_normalScoreBonus = m_gameController.firstReachGoal;
        m_carryingScoreBonus = m_gameController.carryingBonus;*/
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
                }
            }
        }
        Debug.Log("I crossed the finish line, my old score was " + myOldScore + "and my new score was " + myNewScore);


    }
    // Update is called once per frame
    void Update () {
		
	}
}
