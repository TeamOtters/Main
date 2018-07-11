using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerData : MonoBehaviour
{
    public int m_PlayerIndex = 1;
    public int m_CurrentScore = 0;
    public Text scoreText;
    internal Text pInd;
    private GameController m_gameController;
    private PhaseManager m_phaseManager;
    private ScoreManager m_scoreManager;
    private GameObject m_viking;
    private GameObject m_valkyrie;
    public bool m_isGlowing = false;

    // Use this for initialization
    void Start()
    {
        m_gameController = GameController.Instance;
        pInd = GetComponentInChildren<Text>();
        m_phaseManager = m_gameController.phaseManager;
        m_scoreManager = m_gameController.m_scoreManager;

        GetPlayers();

    }

    private void GetPlayers()
    {
        m_viking = GetComponentInChildren(typeof(VikingController), true).gameObject;
        m_valkyrie = GetComponentInChildren(typeof(ValkyrieController), true).gameObject;
    }

    void Update()
    {
        //I'm sending my (this players) score to the scoreManager, there it checks if my score is the highest score and if so, it sets my score as the highest score.
        m_gameController.m_scoreManager.SetHighestScore(m_CurrentScore);


        //am I the highest score? Then, make me glow and be glorious! ( as long as my score isn't the starting score of 0 )
        if (m_CurrentScore >= m_gameController.m_scoreManager.GetHighestScore() && m_CurrentScore != 0)
        {
            //Am I glowing? Only need to set it to  glowing once. We don't want to enable the effect over and over ( we are in Update after all) .
            if (!m_isGlowing)
            {
                //Check if I am a valkyrie or viking, as we need to know which prefab we should reference.
                if (m_viking.activeSelf)
                    m_viking.GetComponent<VikingController>().m_highestScoreEffect.SetActive(true);
                if (m_valkyrie.activeSelf)
                    m_viking.GetComponent<ValkyrieController>().m_highestScoreEffect.SetActive(true);

                //we are in fact glowing! 
                m_isGlowing = true;
            }

        }

        //am I not anymore the one who has the highest score? Make me normal and dull! ( as long as my score isn't the starting score of 0 )
        if (m_CurrentScore < m_gameController.m_scoreManager.GetHighestScore() && m_CurrentScore != 0)
        {
            //Am I glowing? Only need to set it to not glowing once. We don't want to disable the effect over and over. ( we are in Update after all) .
            if (m_isGlowing)
            {
                //Check if I am a valkyrie or viking, as we need to know which prefab we should reference and our score isn't 0. 
                if (m_viking.gameObject.activeSelf)
                    m_viking.GetComponent<VikingController>().m_highestScoreEffect.SetActive(false);
                if (m_valkyrie.activeSelf)
                    m_viking.GetComponent<ValkyrieController>().m_highestScoreEffect.SetActive(true);


                //Not glowing anymore
                m_isGlowing = false;
            }
        }

        if (pInd != null)
        {
            pInd.text = "P" + m_PlayerIndex;

            if (m_PlayerIndex == 1)
            {
                pInd.color = new Color32(170, 253, 255, 255);
            }
            else if (m_PlayerIndex == 2)
            {
                pInd.color = new Color32(153, 232, 157, 255);
            }
            else if (m_PlayerIndex == 3)
            {
                pInd.color = new Color32(212, 142, 108, 255);
            }
            else if (m_PlayerIndex == 4)
            {
                pInd.color = new Color32(253, 146, 214, 255);
            }

            if (scoreText != null)
            {
                scoreText.text = m_CurrentScore.ToString();
            }
        }



    }
}
