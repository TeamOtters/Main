using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class PhaseManager : MonoBehaviour {

    internal bool m_isInPhaseOne = true;
    public bool m_startInPhaseOne = true;
    public PlayerData[] m_players;

    public float m_beforePhase2Delay = 2f;
    public float m_phase2StartDelay = 3f;
    public float m_phase2Duration = 10f;
    public BouncingBall m_bouncingBall;

    private List<int> m_playerScores = new List<int>();
    private bool m_phaseSet = false;
    private bool m_hasActivatedPhase2 = false;
    private bool m_shouldActivatePhase2 = false;

    public Phase2UI m_phase2UI;
    public Canvas m_Phase1UI;

    public GameObject m_dragon;

    private RumbleManager m_rumbleManager;


    void Start ()
    {
        m_Phase1UI.gameObject.SetActive(true);
        //hard coded to 4 atm, if we have a dynamic number of players this might need to change
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);

        m_rumbleManager = GameController.Instance.rumbleManager;
        

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
        // this should be the condition for phase 2 switch - e.g. the ball health
		if(!m_bouncingBall.m_isAlive && !m_phaseSet)
        {
            Debug.Log("ShouldSetUpPhase2");
            StartCoroutine(BeforePhase2Duration(m_beforePhase2Delay));
        }

        // DEBUG, Click to phase 2
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("ShouldSetUpPhase2");
            PhaseTwoSetup();
        }

        //wait for camera shake to end before phase 2 begins
        if (m_shouldActivatePhase2)
        {
            Debug.Log("Starting phase 2");
            if (!m_hasActivatedPhase2 && m_rumbleManager.phaseOneTransformRumbling == false)
            {
                // Begin Phase 2
                Debug.Log("Should start phase 2");
                m_phase2UI.ActivateScoreboard(0);
                Invoke("BeginPhaseTwo", m_phase2StartDelay);
                m_shouldActivatePhase2 = false;
            }
        }
    }   

    // Phase one logic should be contained here
    void PhaseOneSetup()
    {
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
        m_dragon.SetActive(false);
    }
    

    IEnumerator BeforePhase2Duration(float duration)
    {
        m_dragon.SetActive(true);
        yield return new WaitForSeconds(duration);
        PhaseTwoSetup();
        m_dragon.SetActive(false);
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        m_phaseSet = true;
        m_phase2UI.ShowPrompt();
        m_isInPhaseOne = false;
        GameController.Instance.m_currentPhaseState = 2;
        //Adds the current score of the players to the score list
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_playerScores != null && i <= m_playerScores.Count)
            {
                m_playerScores[i] = m_players[i].m_CurrentScore;
            }
            else
            {
                Debug.Log("Cannot find player score list!");
            }

        }
        //local variables to keep track of the highest scores and corresponding indexes
        int highest = 0;
        int highestIndex = -1;

        for (int i = 0; i < m_playerScores.Count; i++)
        {
            //assigns the highest and second highest variables
            if (m_playerScores[i] >= highest)
            {
                /*
                OLD CODE FOR 2 VIKINGS
                second = highest;
                secondIndex = highestIndex;
                */
                highest = m_playerScores[i];
                highestIndex = i + 1;
            }
        }

        //Viking to valkyrie change 
        foreach (PlayerData player in m_players)
        {
            //Accesses the Valkyrie/Viking switch in all players and does the switch to viking
            if (player.m_PlayerIndex != highestIndex) // if the players are not the highest, they should change into valkyries
            {
                var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
                if (mySwitchScript != null)
                {
                    // valkyrie transform camera shake + rumble
                    m_rumbleManager.PhaseOneShake();
                    m_shouldActivatePhase2 = true;

                    mySwitchScript.SwitchToValkyrie();
                }
            }
        }
    }

    void BeginPhaseTwo()
    {
        Debug.Log("Phase2 beginnign");
        m_phase2UI.HidePrompt();
        StartCoroutine(PhaseTwoDuration(m_phase2Duration));
    }

    //Sets the game state back to phase one after a limited time
    IEnumerator PhaseTwoDuration(float phaseDuration)
    {
        Debug.Log("phase2");
        GameController.Instance.cameraManager.SetRaceState(true);

        yield return new WaitForSeconds(phaseDuration);
        PhaseOneSetup();
        m_phaseSet = false;
        m_shouldActivatePhase2 = false;
    }
   
}
