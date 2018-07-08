using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public int m_PlayerIndex = 1;
    public int m_score = 0;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("My player index is" + m_PlayerIndex);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
