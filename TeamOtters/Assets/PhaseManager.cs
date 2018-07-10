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
    private List<int> m_playerScores = new List<int>();
    public float m_phase2Duration = 10f;

    //internal XInputDotNetPure.PlayerIndex[] m_controllerIndex = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four};

    void Start ()
    {
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
		if(Input.GetKeyDown(KeyCode.P))
        {
            PhaseTwoSetup();
        }

        //wait for camera shake to end before phase 2 begins
        foreach(CameraShakeInstance camShakeInstance in CameraShaker.Instance.ShakeInstances)
        {
            if (camShakeInstance.CurrentState == CameraShakeState.Inactive && camShakeInstance.DeleteOnInactive)
            {
                // When camera shake ends, stop the rumble
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                GamePad.SetVibration(PlayerIndex.Three, 0f, 0f);
                GamePad.SetVibration(PlayerIndex.Four, 0f, 0f);                

                // Begin Phase 2
                StartCoroutine(PhaseTwoDuration(m_phase2Duration));
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
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        m_isInPhaseOne = false;
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

        int second = 0;
        int secondIndex = -1;


        for (int i = 0; i < m_playerScores.Count; i++)
        {
            //assigns the highest and second highest variables
            if (m_playerScores[i] >= highest)
            {
                second = highest;
                secondIndex = highestIndex;
                highest = m_playerScores[i];
                highestIndex = i + 1;
            }
            else if (m_playerScores[i] > second)
            {
                second = m_playerScores[i];
                secondIndex = i + 1;
            }
        }

        //for each player, if they are the "highest" or "second highest" index, they should change to valkyries
        foreach (PlayerData player in m_players)
        {
            //Accesses the Valkyrie/Viking switch in all players and does the switch to viking
            if (player.m_PlayerIndex == highestIndex || player.m_PlayerIndex == secondIndex)
            {
                var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
                if (mySwitchScript != null)
                {
                    // valkyrie transform camera shake
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.cameraManager.transform_magnitude,
                                                    GameController.Instance.cameraManager.transform_roughness,
                                                    GameController.Instance.cameraManager.transform_fadeInTime,
                                                    GameController.Instance.cameraManager.transform_fadeOutTime);
                            
                    

                    // vibrate controllers     
                    GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
                    GamePad.SetVibration(PlayerIndex.Two, 1f, 1f);
                    GamePad.SetVibration(PlayerIndex.Three, 1f, 1f);
                    GamePad.SetVibration(PlayerIndex.Four, 1f, 1f);

                    mySwitchScript.SwitchToValkyrie();
                }
            }
        }
    }

    //Sets the game state back to phase one after a limited time
    IEnumerator PhaseTwoDuration(float phaseDuration)
    {
        GameController.Instance.cameraManager.SetRaceState(true);

        yield return new WaitForSeconds(phaseDuration);
        PhaseOneSetup();
    }
   
}
