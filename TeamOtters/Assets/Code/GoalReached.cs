using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class GoalReached : MonoBehaviour {
    public Canvas m_Results;
    private GameController m_gameController = GameController.Instance;
    private int m_carryingScoreBonus;
    private int m_normalScoreBonus;

	// Use this for initialization
	void Start ()
    {
        m_normalScoreBonus = m_gameController.firstReachGoal;
        m_carryingScoreBonus = m_gameController.carryingBonus;
	}

    private void OnTriggerEnter(Collider player)
    {
        int myOldScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
        int myNewScore = 0;
        if (player.gameObject.CompareTag("Valkyrie") && player.gameObject.GetComponent<ValkyrieController>().isCarrying == (true))
        {
            int ID = player.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex;
            player.GetComponentInParent<PlayerData>().SendMessage("AddToScore", m_carryingScoreBonus);
            
            m_Results.gameObject.SetActive(true);
            StartCoroutine("Wait");
            myNewScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
        }
        else if(player.gameObject.CompareTag("Viking") || player.gameObject.CompareTag("Valkyrie"))
        {
            int ID = player.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex;
            player.GetComponentInParent<PlayerData>().SendMessage("AddToScore", m_normalScoreBonus);
            myNewScore = player.gameObject.GetComponentInParent<PlayerData>().m_CurrentScore;
        }

        Debug.Log("I crossed the finish line, my old score was " + myOldScore + "and my new score was " + myNewScore);


    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

    }
    // Update is called once per frame
    void Update () {
		
	}
}
