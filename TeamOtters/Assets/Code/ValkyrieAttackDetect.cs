using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieAttackDetect : MonoBehaviour {
    
    private ValkyrieController m_valkyrie;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    private PlayerData m_playerData;

    // Use this for initialization
    void OnStart ()
    {
        m_valkyrie = GetComponentInParent<ValkyrieController>();
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
        m_playerData = GetComponentInParent<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Valkyrie")
        {
            ValkyrieController otherValkyrieController = other.GetComponent<ValkyrieController>();

            if (otherValkyrieController.isCarrying == true)
            {
                // Make other valkyrie drop pickup if carrying and hit
                other.GetComponent<ValkyrieController>().DropPickup(); //should change if we do a shield mechanic

                // Add to the score of the valkyrie when you successfully attack another valkyrie
                if (m_scoreManager != null)
                {
                    m_scoreManager.AddToScore(m_gameController.hitOpponent, m_playerData.m_PlayerIndex);
                    Debug.Log("I hit a valkyrie and got " + m_gameController.hitOpponent + "points!");
                }
                else
                {
                    Debug.Log("Score manager is null");
                }
            }
            otherValkyrieController.GetStunned();

            
        }
    }
}
