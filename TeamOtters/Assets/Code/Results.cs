using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour {

    public GameObject[] m_rankButtons;
    private int[] m_playerScores;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;


	// Use this for initialization
	void Start () {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
	}
/*
    class ScoreResults
    {
        public ScoreResults(int index, int _score)
        {
            playerIndex = index; 
            score = _score;
        }

        public int playerIndex;
        public int score;
    }
    */

    public void ShowResults ()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
        List<ScoreRanking> scores = new List<ScoreRanking>();

        scores = m_scoreManager.m_ranks;
        /*
        for (int i = 0; i < 4; i++)
        {
            scores.Add(new ScoreResults(m_scoreManager.m_players[i].m_PlayerIndex, m_scoreManager.m_players[i].m_CurrentScore));
        }
        
        scores.Sort((b, a) => a.score.CompareTo(b.score));
        */
        for (int i = 0; i < scores.Count; i++)
        {
            m_rankButtons[i].GetComponent<Text>().text = "P" + scores[i].playerIndex.ToString() + " : " + scores[i].score.ToString();
            m_rankButtons.OrderByDescending(m_rankButtons => m_rankButtons);

            
        if (scores[i].playerIndex == 1)
        {
            m_rankButtons[i].GetComponent<Image>().color = new Color32(47, 94, 0, 100);
        }
        else if (scores[i].playerIndex == 2)
        {
            m_rankButtons[i].GetComponent<Image>().color = new Color32(255, 112, 222, 100);
        }
        else if (scores[i].playerIndex == 3)
        {
            m_rankButtons[i].GetComponent<Image>().color = new Color32(47, 56, 255, 100);
        }
        else if (scores[i].playerIndex == 4)
        {
            m_rankButtons[i].GetComponent<Image>().color = new Color32(207, 122, 1, 100);
        }
        
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
