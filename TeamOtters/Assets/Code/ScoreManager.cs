using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class ScorePointInfo
{
    public static int playerContiniousScore = 2;
    public static int scoreItem = 5;
    public static int bounceHit = 3;
    public static int firstReachGoal = 10;
    public static int carryingBonus = 20;
    public static int hitOpponent = 2;
}


public class ScoreManager : MonoBehaviour {

    internal PlayerData[] m_players= { null, null, null, null };
    internal int [] m_playerScores = { 0, 0, 0, 0 };
    private int highest = 0;
    private int highestScore = 0;
    private GlowEffect m_glowEffectScript;
    public GameObject m_glowEffect;
    public Text[] scoreText;
    //public static ScorePointInfo scorePointInfo { get; }
    

    //Am the players score I'm getting Higher then the current highestScore? If so, set that score the new highest score! 
    public void SetHighestScore(int score)
    {
        if (score > highestScore)
        {
            highestScore = score;
        }
    }

    //Easy way for the player to see if they are the highest score!
    public int GetHighestScore()
    {
        return highestScore;
    }

    public void AddToScore(int points, int playerIndex)
    {

        m_players[playerIndex-1].m_CurrentScore += points;
       //Debug.Log("Player " + (playerIndex) + " - Current MANAGER Score : " + m_players[playerIndex - 1].m_CurrentScore);

        //Sending playerIndex of player that just scored
        //SetHighestPlayerScore(playerIndex-1);

    }

   

    //Additions by Vivienne

    public void Update()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            scoreText[i].text=m_players[i].m_CurrentScore.ToString();
        }
            
    }

    /*
    private void SetHighestPlayerScore(int scoringPlayer)
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_playerScores[i] != 0 && i <= m_playerScores.Length)
            {
                m_playerScores[i] = m_players[i].m_CurrentScore;
                Debug.Log(i + " is indexnumber for Player" + scoringPlayer + m_players[i]);
            }
            else
            {
                Debug.Log("Cannot find player score list!");
            }

        }
        //local variables to keep track of the highest scores and corresponding indexes

        for (int i = 0; i < m_playerScores.Length; i++)

        {
         

            //check who has the highest score and give them gloweffect
            if (i == scoringPlayer && m_playerScores[i] >= highest && m_glowEffectScript == null)
            {
                highest = m_playerScores[i];

                Instantiate(m_glowEffect, m_players[i].GetComponentInChildren<VikingController>().gameObject.transform);
                Debug.Log(m_players[i] + " says I EARNED IT!");
               m_glowEffectScript = m_players[i].GetComponentInChildren<GlowEffect>();
            }
            if (i == scoringPlayer && m_playerScores[i] >= highest && m_glowEffectScript != null || i != scoringPlayer && m_playerScores[i] >= highest && m_glowEffectScript != null)
            {
                highest = m_playerScores[i];
                Debug.Log(m_players[i] + " says I ALREADY HAVE THE GLOW");
            }

            if (m_playerScores[i] == highest)
                {
                     Debug.Log(m_players[i] + "Am I a Zero??");
                }
            else
            {

                if (m_playerScores[i] < highest && m_glowEffectScript != null)
                {
                    Destroy(m_glowEffectScript.gameObject);
                    Debug.Log(m_players[i] + "NOT IN THE LEAD ANYMORE :( ");
                }

                else
                {
                    Debug.Log(m_players[i] + " I NEED MORE SCORE!");
                }
               
            }
        }
    }*/




}
