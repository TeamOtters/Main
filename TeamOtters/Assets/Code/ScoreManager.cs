using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    internal PlayerData[] m_players= { null, null, null, null };
    internal int [] m_playerScores = { 0, 0, 0, 0 };

    public GameObject m_glowEffect;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddToScore(int points, int playerIndex)
    {

        m_players[playerIndex-1].m_CurrentScore += points;
        Debug.Log("Player " + playerIndex + " - Current MANAGER Score : " + m_players[playerIndex - 1].m_CurrentScore);

        SetHighestPlayerScore();

    

        //int maxValue = m_highestScore.Max();

        /*for (int i = 0; i < m_phaseManager.m_players.Length; i++)
        {

        }


        if (m_CurrentScore == maxValue)
        {
            //GameObject effect = GetComponentsInChildren<VikingPlayer(Clone)>(Instantiate(m_glowEffect, transform.position, Quaternion.identity));
            //GameObject fisk = Player_1.transform.GetChild(0).gameObject;
            Debug.Log(m_PlayerIndex + " HAS THE HIGHEST SCOOOOOOOOOOOOORE!!!!");
        }*/

    }

    private void SetHighestPlayerScore()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_playerScores != null && i <= m_playerScores.Length)
            {
                m_playerScores[i] = m_players[i].m_CurrentScore;
            }
            else
            {
                Debug.Log("Cannot find player score list!");
            }

        }
        //local variables to keep track of the highest scores and corresponding indexes
        int highest = 0;
        int highestIndex = -1;

        for (int i = 0; i < m_playerScores.Length; i++)
        {
            //check who has the highest score
            if (m_playerScores[i] >= highest)
            {
                highest = m_playerScores[i];
                highestIndex = i + 1;
                GameObject effect = (GameObject)Instantiate(m_glowEffect,m_players[i].GetComponentInChildren<VikingController>().gameObject.transform);
            }
            else 
            {

                GlowEffect gloweffect = m_players[i].GetComponentInChildren<GlowEffect>();

                if (gloweffect != null)
                {
                    Destroy(gloweffect.gameObject);
                    Debug.Log("I'm not in the LEAD :( ");
                }

                else
                {
                    Debug.Log("Gloweffect is DEAD!");
                }
               
            }
        }
    }




}
