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

        m_players = FindObjectsOfType<PlayerData>();
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

    void PhaseTwoSetup()
    {

        foreach(PlayerData player in m_players)
        {
            int currentPlayerScore = player.m_CurrentScore;
            m_playerScores.Add(currentPlayerScore);
        }

    }
    void TwoHighest(out float highest, out float second, List<int> scores)
    {
        highest = Mathf.NegativeInfinity;
        second = Mathf.NegativeInfinity;
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i] >= highest)
            {
                second = highest;
                highest = scores[i];
            }
            else if (scores[i] > second)
            {
                second = scores[i];
            }
        }
    }
}
