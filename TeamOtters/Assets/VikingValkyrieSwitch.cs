using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingValkyrieSwitch : MonoBehaviour {

    
    public GameObject m_vikingCharacter;
    public GameObject m_valkyrieCharacter;
    
    private bool m_isValkyrie;

	// Use this for initialization
	void Start ()
    {
        //sets viking at start
        SwitchToViking();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //running a function that listens for the switch condition every frame
        SwitchConditionListener(); 
	}

    void SwitchConditionListener()
    {
        //condition for switch
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // check which switch function to use
           if (m_isValkyrie)
            {
                SwitchToViking();
            }
            else
            {
                SwitchToValkyrie();
            }
        }
    }

    // Switch to Viking
    void SwitchToViking()
    {
        Debug.Log("I am a Viking!");
        m_isValkyrie = false;
    }

    // Switch to Valkyrie
    void SwitchToValkyrie()
    {
        Debug.Log("I am a Valkyrie!");
        m_isValkyrie = true;
    }
}
