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
    public float m_beforePhase2Delay = 2f;
    public float m_phase2StartDelay = 3f;
    public float m_phase2Duration = 10f;
    
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
    private bool m_hasActivatedPhase2 = false;
    private bool m_shouldActivatePhase2 = false;
    private RumbleManager m_rumbleManager;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;

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
    }

    // the condition for phase 2 switch - ball health and debug command
    private void CheckPhaseSwitch()
    {
        
        if ((!m_bouncingBall.m_isAlive && !m_phaseSet)|| Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TransformationDuration(m_beforePhase2Delay));
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
    IEnumerator TransformationDuration(float duration)
    {
        m_dragon.SetActive(true);
        m_rumbleManager.PhaseOneShake();
        yield return new WaitForSeconds(duration);
        PhaseTwoSetup();
        m_dragon.SetActive(false);
    }

    IEnumerator CharacterTransformation()
    {
        yield return null;
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        m_phaseSet = true;//prevents phase switch from happening every frame
        m_phase2UI.ShowPrompt();
        m_isInPhaseOne = false; //for other scripts to listen to
        GameController.Instance.m_currentPhaseState = 2;

        foreach (PlayerData player in m_players)
        {
            //Accesses the Valkyrie/Viking switch in all players and does the switch to viking
            if (player.m_PlayerIndex != m_scoreManager.m_ranks[0].playerIndex) // if the players are not the highest, they should change into valkyries
            {
                var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
                if (mySwitchScript != null)
                {
                    // valkyrie transform camera shake + rumble
                    m_shouldActivatePhase2 = true;
                    mySwitchScript.SwitchToValkyrie();
                    m_phase2UI.HidePrompt();
                    m_phase2UI.ActivateScoreboard(0);
                    GameController.Instance.cameraManager.SetRaceState(true);
                }
            }
        }

    }
   
}
