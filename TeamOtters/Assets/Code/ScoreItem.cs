using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour {

    private ScoreManager m_scoreManagerCloud;
    private GameController m_gameController;
    int m_scoringPlayer;

    private void Start()
    {
        m_gameController = GameController.Instance;
        m_scoreManagerCloud = m_gameController.m_scoreManager;
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.layer == 10 || collisionInfo.gameObject.layer == 11)
        {
             m_scoringPlayer = collisionInfo.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex;

            m_scoreManagerCloud.AddToScore(10, m_scoringPlayer);
            Debug.Log("Player" + m_scoringPlayer + "Scored 10 points for  touching Cloud");
            Destroy(gameObject);
        }
        
    }
}
//gameObject.CompareTag("Viking") || collisionInfo.gameObject.CompareTag("Valkyrie"))