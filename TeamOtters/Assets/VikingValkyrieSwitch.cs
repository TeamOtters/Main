using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingValkyrieSwitch : MonoBehaviour {

    public bool m_startViking = true;

    private GameObject m_vikingCharacter;
    private GameObject m_valkyrieCharacter;
    
    private bool m_isValkyrie;

	// Use this for initialization
	void Start ()
    {
        m_vikingCharacter = GetComponentInChildren<VikingController>().gameObject;
        m_valkyrieCharacter = GetComponentInChildren<ValkyrieAnimController>().gameObject;

        //sets viking at start
        if (m_startViking)
        {
            SwitchToViking();

        }
        else
        {
            SwitchToValkyrie();
        }
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

        //Activating viking, childing valkyrie to viking and deactivating valkyrie
        m_isValkyrie = false;
        m_vikingCharacter.SetActive(true);
        m_vikingCharacter.transform.parent = this.gameObject.transform;
        m_valkyrieCharacter.transform.parent = m_vikingCharacter.transform;
        m_valkyrieCharacter.SetActive(false);
    }

    // Switch to Valkyrie
    void SwitchToValkyrie()
    {
        Debug.Log("I am a Valkyrie!");

        //Activating valkyrie, childing viking to valkyrie and deactivating viking
        m_isValkyrie = true;
        m_valkyrieCharacter.SetActive(true);
        m_valkyrieCharacter.transform.parent = this.gameObject.transform;
        m_vikingCharacter.transform.parent = m_valkyrieCharacter.transform;
        m_vikingCharacter.SetActive(false);
    }
}
