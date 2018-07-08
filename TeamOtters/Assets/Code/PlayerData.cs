using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{

    public int m_PlayerIndex = 1;
    public int m_CurrentScore = 0;
    public Text scoreText;

    // Use this for initialization
    void Start()
    { 	
    }

    // Update is called once per frame
    void Update ()
    {
        scoreText.text = m_CurrentScore.ToString();

        Debug.Log("Player " + m_PlayerIndex + " - Current Score : " + m_CurrentScore);
	}
}
