using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour {

    [Header ("Settings")]
    public bool m_startInPhaseOne = true;
    public float m_playerTransformationDuration = 0.2f;
    public float m_phaseChangeDuration = 2f;
    public float m_phase2Duration = 10f;
    public float m_characterUpwardsMoveSpeed = 1.2f;

    
    [Header ("Objects")]
    public BouncingBall m_bouncingBall;
    public GameObject m_dragon;
    public GameObject m_Level;

    [Header("UI")]
    public Phase2UI m_phase2UI;
    public Canvas m_Phase1UI;

    [HideInInspector]
    public PlayerData[] m_players;
    internal bool m_isInPhaseOne = true;
    private List<int> m_playerScores = new List<int>();
    private bool m_phaseSet = false;
    private RumbleManager m_rumbleManager;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    internal bool m_phaseTransformationActive = false;
    private bool m_isWaiting = false;

    void Start ()
    {
        m_Phase1UI.gameObject.SetActive(true);
        //hard coded to 4 atm, if we have a dynamic number of players this might need to change
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);

        m_gameController = GameController.Instance;
        m_rumbleManager = m_gameController.rumbleManager;
        m_scoreManager = m_gameController.m_scoreManager;
        

        //allows the devs to set the starting phase
        if (m_startInPhaseOne == true)
        {
            PhaseOneSetup();
        }
        else
        {
            PhaseTwoSetup();
        }
        m_dragon.SetActive(false);
        
	}	

	void Update ()
    {
        CheckPhaseSwitch();
        if(m_isWaiting)
        {
            foreach (PlayerData player in m_players)
            {
                GameObject viking = player.GetComponentInChildren(typeof(VikingController), true).gameObject;
                viking.transform.Translate(Vector3.up * m_characterUpwardsMoveSpeed * Time.deltaTime);
            }
        }
    }

    // the condition for phase 2 switch - ball health and debug command
    private void CheckPhaseSwitch()
    {
        
        if ((!m_bouncingBall.m_isAlive && !m_phaseSet)|| Input.GetKeyDown(KeyCode.P))
        {
            m_phaseSet = true;
            StartCoroutine(PhaseChangeStart(m_phaseChangeDuration));
        }
        
    }
    
    //phase 1 setup - called on start
    void PhaseOneSetup()
    {
        m_dragon.SetActive(false);
        m_isInPhaseOne = true;
        foreach(PlayerData player in m_players)
        {
            //Accesses the Valkyrie/Viking switch in all players and does the switch to viking
            var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
            if (mySwitchScript!=null && mySwitchScript.m_startViking == true)
            {
                mySwitchScript.SwitchToViking();
            }
        }
        //respawns the bouncing ball in phase 1 if we want
        if(!m_bouncingBall.m_isAlive)
        {
            m_bouncingBall.Respawn();
            Debug.Log("Bouncing ball respawn triggered");
        }
    }
    
    //dramatic moment before phase 2 start
    IEnumerator PhaseChangeStart(float duration)
    {
        m_isWaiting = true;
        m_phaseTransformationActive = true;
        m_dragon.SetActive(true);
        m_rumbleManager.PhaseOneShake();
        GameObject viking;
        foreach (PlayerData player in m_players)
        {
            viking = player.GetComponentInChildren(typeof(VikingController),true).gameObject;
            viking.GetComponent<VikingController>().enabled = false;

            //StartCoroutine(MoveViking(viking));
        }
        
        yield return new WaitForSeconds(duration);
        m_isWaiting = false;
        m_dragon.SetActive(false);
        m_phaseTransformationActive = false;
        StartCoroutine(CharacterTransformation(0));
    }

    //Character Transformation and Character Transformation Delay loops around so that each character gets spawned together with their UI element in a delay.
    IEnumerator CharacterTransformation(int i)
    {
        int playerIndex = m_scoreManager.m_ranks[i].playerIndex;
        var mySwitchScript = m_players[playerIndex - 1].gameObject.GetComponent<VikingValkyrieSwitch>();
        if (m_players[playerIndex-1].m_PlayerIndex != m_scoreManager.m_ranks[0].playerIndex) // if the players are not the highest, they should change into valkyries
        {
            if (mySwitchScript != null)
            {
                // valkyrie transform camera shake + rumble
                mySwitchScript.SwitchToValkyrie();

            }
        }
        else
        {
            mySwitchScript.SwitchToViking();
        }
        GameObject viking = m_players[playerIndex - 1].GetComponentInChildren(typeof(VikingController), true).gameObject;
        viking.GetComponent<VikingController>().enabled = true;

        m_phase2UI.ActivateScoreboard(i);
        StartCoroutine(CharacterTransformationDelay(i));
        yield return null;
    }

    IEnumerator CharacterTransformationDelay(int i)
    {
        Debug.Log("Delay");
        yield return new WaitForSeconds(m_playerTransformationDuration);
        if (i < m_players.Length -1)
        {
            i++;
            StartCoroutine(CharacterTransformation(i));
        }
        else
        {
            PhaseTwoSetup();
        }
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        Debug.Log("set up phase 2!");
        m_phaseSet = true;//prevents phase switch from happening every frame
        m_phase2UI.ShowPrompt();
        m_isInPhaseOne = false; //for other scripts to listen to
        GameController.Instance.m_currentPhaseState = 2;

        m_phase2UI.HidePrompt();
        GameController.Instance.cameraManager.SetRaceState(true);

    }
   
}
