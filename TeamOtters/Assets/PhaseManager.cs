using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour {

    public bool m_isInPhaseOne = true;
    public PlayerData[] m_players;
    public List<int> m_playerScores;

	void Start ()
    {
        
        if (m_isInPhaseOne == true)
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
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            PhaseTwoSetup();
        }
	}

    // Set all characters to viking
    void PhaseOneSetup()
    {
        foreach(PlayerData player in m_players)
        {
            var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
            if (mySwitchScript!=null)
            {
                mySwitchScript.SwitchToViking();
            }
        }
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        //Adds the current score of the players to the score list
        for (int i = 0; i < m_players.Length; i++)
        {
            m_playerScores[i] = m_players[i].m_CurrentScore;

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

        Debug.Log("Highest value is " + highest + "with index " + highestIndex + ". Second highest value is " + second + "with index " + secondIndex);

        //for each player, if they are the "highest" or "second highest" index, they should change to valkyries
        foreach(PlayerData player in m_players)
        {
            if (player.m_PlayerIndex == highestIndex || player.m_PlayerIndex == secondIndex)
            {
                player.GetComponent<VikingValkyrieSwitch>().SwitchToValkyrie();
                Debug.Log(player.m_PlayerIndex);
            }
        }
    }
   
}
