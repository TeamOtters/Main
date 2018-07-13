using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{

    public RectTransform canvasRect;
    public GameObject[] m_playerTexts;
    public float m_yOffset;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    // Use this for initialization
    void Start ()
    {
        m_gameController = GameController.Instance;
	}
	
	// Update is called once per frame

	void Update ()
    {
		
	}

    public void DisplayScoreText()
    {

    }
}
