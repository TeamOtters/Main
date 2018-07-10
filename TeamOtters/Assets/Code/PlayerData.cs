using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerData : MonoBehaviour
{

    public int m_PlayerIndex = 1;
    public int m_CurrentScore = 0;
    internal Text scoreText;


    private GameController m_gameController = GameController.Instance;
    private PhaseManager m_phaseManager;

    private int m_otherPlayerIndex;

    int[] m_highestScore = { 0, 0, 0, 0 };
    int maxValue;
    public GameObject m_glowEffect;


    // Use this for initialization
    void Start()
    { 	
        scoreText = GetComponentInChildren<Text>();
        m_phaseManager = m_gameController.phaseManager;
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


    
}
