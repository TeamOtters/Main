using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Animations; 

public static class ScorePointInfo
{
    //public static int playerContiniousScore = 2;
    public static int vikingContiniousScore = 2;
    public static int valkyrieContiniousScore = 10;
    public static int scoreItem = 5;
    public static int bounceHit = 5;
    public static int firstReachGoal = 10;
    public static int carryingBonus = 20;
    public static int hitOpponent = 2;
    
}

   

public class ScoreRanking
{
    public ScoreRanking(int index, int _score)
    {
        playerIndex = index;
        score = _score;
    }

    public int playerIndex;
    public int score;
}


public class ScoreManager : MonoBehaviour {

    internal PlayerData[] m_players= { null, null, null, null };
    internal int [] m_playerScores = { 0, 0, 0, 0 };
    public List<ScoreRanking> m_ranks = new List<ScoreRanking>(); //this is the list where players will be arranged according to rank. m_ranks[0] will be the 1st place player and their score, [1] second place and so on.
    private int highestScore = 0;
    private GlowEffect m_glowEffectScript;
    private GameController m_gameController;
    public GameObject m_glowEffect;
    public Text[] scoreText;
    

    public PlayerScoreUI m_playerScoreUI;

    //public static ScorePointInfo scorePointInfo { get; }

    private void Start()
    {
        m_gameController = GameController.Instance;
        for(int i=0; i< m_players.Length; i++)
        {
            m_ranks.Add(new ScoreRanking(0, 0));
        }
        UpdateScoreRanking();
        

    }
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
        scoreText[playerIndex - 1].GetComponent<Animation>().Play(); 
        
       
    
        //Debug.Log("Player " + (playerIndex) + " - Current MANAGER Score : " + m_players[playerIndex - 1].m_CurrentScore);
        //Sending playerIndex of player that just scored
        //SetHighestPlayerScore(playerIndex-1);
    }

   

    //Additions by Vivienne

    public void Update()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            scoreText[i].text = m_players[i].m_CurrentScore.ToString();

        }
        
        //m_ranks will be a reflection of the players rank

        UpdateScoreRanking();
        if(Input.GetKeyDown(KeyCode.A))
        {
        }

    }

 

    private void UpdateScoreRanking()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            ScoreRanking current = m_ranks[i];
            current.playerIndex = m_players[i].m_PlayerIndex;
            current.score = m_players[i].m_CurrentScore;
        }

        m_ranks.Sort((b, a) => a.score.CompareTo(b.score));
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
