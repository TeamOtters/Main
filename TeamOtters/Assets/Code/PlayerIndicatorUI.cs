using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerIndicatorUI : MonoBehaviour
{
    public RectTransform canvasRect;
    public GameObject[] m_playerTexts;
    public float m_yOffset;
    private GameController m_gameController;


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
            string playerIndicator = myText.text;
            int playerIndex = m_gameController.phaseManager.m_players[i].m_PlayerIndex;
            playerIndicator = ("P" + m_gameController.phaseManager.m_players[i].m_PlayerIndex);
            
            //Raven's colors here
            if (playerIndex == 1)
            {
                myText.color = new Color32(170, 253, 255, 255);
            }
            else if (playerIndex == 2)
            {
                myText.color = new Color32(153, 232, 157, 255);
            }
            else if (playerIndex == 3)
            {
                myText.color = new Color32(212, 142, 108, 255);
            }
            else if (playerIndex == 4)
            {
                myText.color = new Color32(253, 146, 214, 255);
            }
            MoveUIToPlayer(i);
            
        }
	}

    private void MoveUIToPlayer(int index)
    {
        RectTransform rectTransform = m_playerTexts[index].GetComponent<RectTransform>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(m_gameController.phaseManager.m_players[index].GetComponentInChildren(typeof(VikingController), true).transform.position);
        m_playerTexts[index].transform.position = new Vector2(screenPoint.x, screenPoint.y + m_yOffset);
        //rectTransform.anchoredPosition = (screenPoint - canvasRect.sizeDelta / 2);
        /*
        var wantedPos = Camera.main.WorldToViewportPoint(m_gameController.phaseManager.m_players[index].GetComponentInChildren(typeof(VikingController), true).transform.position);
        m_playerTexts[index].transform.position = new Vector3(wantedPos.x, wantedPos.y, 0f);
        */

        /*
        Debug.Log("Setting position");
        //m_playerTexts[index].transform.position = m_gameController.phaseManager.m_players[index].GetComponentInChildren(typeof(VikingController), true).transform.position;
        RectTransform rectTransform = m_playerTexts[index].GetComponent<RectTransform>();
        Vector2 playerViewportPos = Camera.main.WorldToViewportPoint(m_gameController.phaseManager.m_players[index].gameObject.GetComponentInChildren(typeof (VikingController),true).gameObject.transform.position);
        Vector2 playerScreenPos = new Vector2(((playerViewportPos.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f)),((playerViewportPos.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = new Vector2(playerScreenPos.x, playerScreenPos.y + m_yOffset);
        */

        //m_playerTexts[index].transform.position = new Vector3(playerScreenPos.x, playerScreenPos.y + m_yOffset, 0f);
    }
}
