using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unity1010PublicObjects : MonoBehaviour {

    [HideInInspector]
    public GameObject m_theObject;

    private float m_currentScore;
    public float m_player1Score;

    private void Start()
    {

        m_theObject.SetActive(true);
    }


    public void SetScore(float score) 
    {
        m_player1Score = m_currentScore + score;
        m_currentScore = m_player1Score;
    }
}
