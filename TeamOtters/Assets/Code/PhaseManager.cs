using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class PhaseManager : MonoBehaviour {

    public bool m_isInPhaseOne = true;
    public bool m_startInPhaseOne = true;
    public PlayerData[] m_players;
    public float m_phase2Duration = 10f;
    public float m_phase2Delay = 3f;
    public BouncingBall m_bouncingBall;

    private List<int> m_playerScores = new List<int>();
    private bool m_phaseSet = false;
    private bool m_hasStartedPhase2 = false;

    public Phase2UI m_phase2UI;
    public Canvas m_Phase1UI;

    void Start ()
    {
        m_Phase1UI.gameObject.SetActive(true);
        //hard coded to 4 atm, if we have a dynamic number of players this might need to change
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        m_playerScores.Add(0);
        

        //allows the devs to set the starting phase
        if (m_startInPhaseOne == true)
        {
            PhaseOneSetup();
        }
        else
        {
            PhaseTwoSetup();
        }
       
        
	}	

	void Update ()
    {
        // this should be the condition for phase 2 switch - e.g. the ball health
		if(!m_bouncingBall.m_isAlive && !m_phaseSet)
        {
            Debug.Log("ShouldSetUpPhase2");
            PhaseTwoSetup();
        }

        // DEBUG, Click to phase 2
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("ShouldSetUpPhase2");
            PhaseTwoSetup();
        }

        //wait for camera shake to end before phase 2 begins
        if (GameController.Instance.rumbleManager.transformShakeComplete == true && !m_hasStartedPhase2)
       {
            // Begin Phase 2
            m_hasStartedPhase2 = true;
            m_phase2UI.ActivateScoreboard(0);
            Invoke("BeginPhaseTwo", m_phase2Delay);      
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
    }

    void BeginPhaseTwo()
    {
        m_phase2UI.HidePrompt();
        StartCoroutine(PhaseTwoDuration(m_phase2Duration));
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        m_phaseSet = true;
        Debug.Log("I am in phase 2!)");
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
                    GameController.Instance.rumbleManager.TransformShakeStart();

                    mySwitchScript.SwitchToValkyrie();
                }
            }
        }
    }

    //Sets the game state back to phase one after a limited time
    IEnumerator PhaseTwoDuration(float phaseDuration)
    {
        Debug.Log("phase2");
        GameController.Instance.cameraManager.SetRaceState(true);

        yield return new WaitForSeconds(phaseDuration);
        PhaseOneSetup();
        m_phaseSet = false;
    }
   
}
