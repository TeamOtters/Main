using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingValkyrieSwitch : MonoBehaviour {

    public bool m_startViking = true;

    private GameObject m_vikingCharacter;
    private GameObject m_valkyrieCharacter;
    private PlayerData m_playerData;
    private bool m_isValkyrie;
    private bool m_shouldSwitch = false;

	// Use this for initialization
	void Start ()
    {
        //initialize all components
        m_vikingCharacter = GetComponentInChildren<VikingController>().gameObject;
        m_valkyrieCharacter = GetComponentInChildren<ValkyrieController>().gameObject;
        m_playerData = GetComponent<PlayerData>();

        //sets starting state according to startViking variable
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
        if (m_playerData.m_CurrentScore > 5 && m_shouldSwitch)
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
        m_shouldSwitch = false;

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
        m_shouldSwitch = false;

        //Activating valkyrie, childing viking to valkyrie and deactivating viking
        m_isValkyrie = true;
        m_valkyrieCharacter.SetActive(true);
        m_valkyrieCharacter.transform.parent = this.gameObject.transform;
        m_vikingCharacter.transform.parent = m_valkyrieCharacter.transform;
        m_vikingCharacter.SetActive(false);
    }
}
