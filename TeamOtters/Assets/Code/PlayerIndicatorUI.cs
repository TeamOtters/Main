﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerIndicatorUI : MonoBehaviour
{
    public RectTransform canvasRect;
    public GameObject[] m_playerTexts;
    public GameObject[] m_gainScore; 
    public float m_yOffset;
    public float m_yRPGOffset;
    public float m_xOffset;
    private GameController m_gameController;
    private ScoreManager m_scoreManager; 


	void Start ()
    {
        m_gameController = GameController.Instance;
	}
	
	// Update is called once per frame
	void Update ()
    {

        for (int i = 0; i < m_playerTexts.Length; i++)
        {
            Text myText = m_playerTexts[i].GetComponent<Text>();
            int playerIndex = m_gameController.phaseManager.m_players[i].m_PlayerIndex;
            myText.text = ("P" + playerIndex.ToString());

                       
            //Raven's colors here
            if (playerIndex == 1)
            {
                myText.color = new Color32(47, 94, 0, 255);
            }
            else if (playerIndex == 2)
            {
                myText.color = new Color32(255, 112, 222, 255);
            }
            else if (playerIndex == 3)
            {
                myText.color = new Color32(47, 56, 255, 255);
            }
            else if (playerIndex == 4)
            {
                myText.color = new Color32(207, 122, 1, 255);
            }
            MoveIDUIToPlayer(i);
            SetActiveState(i);
        }
	}

    
    private void SetActiveState(int playerIndex)
    {
        m_playerTexts[playerIndex].gameObject.SetActive(m_gameController.phaseManager.m_players[playerIndex].gameObject.activeSelf);

    }

    public void MoveIDUIToPlayer(int index)
    {
        RectTransform rectTransform = m_playerTexts[index].GetComponent<RectTransform>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(m_gameController.phaseManager.m_players[index].GetComponentInChildren(typeof(VikingController), true).transform.position);
        m_playerTexts[index].transform.position = new Vector2(screenPoint.x, screenPoint.y + m_yOffset);

    }

    public void MoveRPGScoreToPlayer(int index)
    {
        RectTransform rectTransform = m_gainScore[index].GetComponent<RectTransform>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(m_gameController.phaseManager.m_players[index].GetComponentInChildren(typeof(VikingController), true).transform.position);
        m_gainScore[index].transform.position = new Vector2(screenPoint.x + m_xOffset, screenPoint.y + m_yRPGOffset);

    }
}


