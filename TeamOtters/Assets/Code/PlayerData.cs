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



    private GameController m_gameController;
    private PhaseManager m_phaseManager;

    private int m_otherPlayerIndex;

    int[] m_highestScore = { 0, 0, 0, 0 };
    int maxValue;
    public GameObject m_glowEffect;

    private ScoreManager m_scoreManager;
    private GameObject m_viking;
    private GameObject m_valkyrie;
    private GameObject m_glowViking;
    private GameObject m_glowValkyrie;


    // Use this for initialization
    void Start()
        m_gameController = GameController.Instance;
        scoreText = GetComponentInChildren<Text>();
        m_phaseManager = m_gameController.phaseManager;
        m_scoreManager = m_gameController.m_scoreManager;

        StartCoroutine("LateStart");
       
      
    }

    IEnumerator LateStart ()
    {
        yield return new WaitForSecondsRealtime(3f);
        m_viking = GetComponentInChildren<VikingController>().gameObject;
        m_valkyrie = GetComponentInChildren<ValkyrieController>().gameObject;
        m_glowViking = m_viking.GetComponent<VikingController>().m_highestScoreEffect;
        Debug.Log("LAteStart");
        //m_glowValkyrie = GetComponentInChildren<ValkyrieController>().m_highestScoreEffect;
    }

    // Update is called once per frame
    void Update ()
    {

        print("Highest:" + m_gameController.m_scoreManager.GetHighestScore());


        m_gameController.m_scoreManager.SetHighestScore(m_CurrentScore);

        if (m_CurrentScore >= m_gameController.m_scoreManager.GetHighestScore())
        {
            print("Player:" + m_PlayerIndex.ToString() + "has Highest Score of:" + m_gameController.m_scoreManager.GetHighestScore() + " WOOOOOOOW!!");

            if (m_viking.activeSelf && m_CurrentScore != 0)
            {
                if(!m_glowViking.activeSelf)
                    m_glowViking.SetActive(true);
            }
           // if (m_valkyrie.activeSelf)
             //   m_glowValkyrie.SetActive(true);

        }

        if (m_CurrentScore < m_gameController.m_scoreManager.GetHighestScore())
        {
            print("Player:" + m_PlayerIndex.ToString() + "does not have highest score. BUUUUUUUU!");


            if (m_viking.gameObject.activeSelf)
            {
                if(m_glowViking.activeSelf)
                    m_glowViking.SetActive(false);
            }
            //if (m_valkyrie.activeSelf)
              //  m_glowValkyrie.SetActive(false);

        }
        if (scoreText != null)

            {
                scoreText.text = "P" + m_PlayerIndex;
                //scoreText.text = "Player " + m_PlayerIndex + ": " + m_CurrentScore.ToString() + " score";
            }
	}


    
}
