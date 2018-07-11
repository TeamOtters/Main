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

    public void ShowResults ()
    {
        m_gameController = GameController.Instance;
        m_scoreManager = m_gameController.m_scoreManager;
        List<ScoreResults> scores = new List<ScoreResults>();
        for (int i = 0; i < 4; i++)
        {
            scores.Add(new ScoreResults(m_scoreManager.m_players[i].m_PlayerIndex, m_scoreManager.m_players[i].m_CurrentScore));
        }

        scores.Sort((b, a) => a.score.CompareTo(b.score));

        for (int i = 0; i < scores.Count; i++)
        {
            m_rankButtons[i].GetComponent<Text>().text = "P" + scores[i].playerIndex.ToString() + " : " + scores[i].score.ToString();
            m_rankButtons.OrderByDescending(m_rankButtons => m_rankButtons);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
