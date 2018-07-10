using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{

    public int m_PlayerIndex = 1;
    public int m_CurrentScore = 0;
    internal Text scoreText;



    private int m_otherPlayerIndex;

    // Use this for initialization
    void Start()
    { 	
        scoreText = GetComponentInChildren<Text>();



    }

    // Update is called once per frame
    void Update ()
    {

        if (scoreText != null)

            {
                scoreText.text = "P" + m_PlayerIndex;
                //scoreText.text = "Player " + m_PlayerIndex + ": " + m_CurrentScore.ToString() + " score";
            }
	}

  

    void AddToScore(int points)
    {
        m_CurrentScore += points;
        Debug.Log("Player " + m_PlayerIndex + " - Current Score : " + m_CurrentScore);
    }
}
