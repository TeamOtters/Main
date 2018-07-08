using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScoreAssigner : MonoBehaviour {

    private PlayerData[] m_players;

	void Start ()
    {
        m_players = FindObjectsOfType<PlayerData>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            int indexToReward = Random.Range(0, 3);
            m_players[indexToReward].m_CurrentScore = m_players[indexToReward].m_CurrentScore + 1;
        }
	}
}
