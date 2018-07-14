﻿using System.Collections;
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
    public Text[] m_scoreBoardText;

    private PlayerUI m_playerUI;

    //public static ScorePointInfo scorePointInfo { get; }

    private void Start()
    {
        m_gameController = GameController.Instance;
        m_playerUI = GameController.Instance.playerUI;
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
        m_scoreBoardText[playerIndex - 1].GetComponent<Animation>().Play();

        //RPG Hit!

        m_playerUI.MoveRPGScoreToPlayer(playerIndex -1);
        m_playerUI.m_gainScoreTexts[playerIndex - 1].gameObject.SetActive(true);
        m_playerUI.m_gainScoreTexts[playerIndex - 1].GetComponent<Text>().text = ("+" + points.ToString());
        m_playerUI.m_gainScoreTexts[playerIndex - 1].GetComponent<Animation>().Play();


    }


    public void Update()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_scoreBoardText[i].text = m_players[i].m_CurrentScore.ToString();

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
            /*
            if (current.playerIndex == 1)
            {
                this.GetComponentInParent<Image>().color = new Color32(47, 94, 0, 180);
            }
            else if (current.playerIndex == 2)
            {
                this.GetComponentInParent<Image>().color = new Color32(255, 112, 222, 180);
            }
            else if (current.playerIndex == 3)
            {
                this.GetComponentInParent<Image>().color = new Color32(47, 56, 255, 180);
            }
            else if (current.playerIndex == 4)
            {
                this.GetComponentInParent<Image>().color = new Color32(207, 122, 1, 180);
            }
            */
        }

        m_ranks.Sort((b, a) => a.score.CompareTo(b.score));

        //Raven's colors here
        
    }

}
