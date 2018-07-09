﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingValkyrieSwitch : MonoBehaviour {

    public bool m_startViking = true;

    private GameObject m_vikingCharacter;
    private GameObject m_valkyrieCharacter;
    private PlayerData m_playerData;
    private bool m_isValkyrie;
    private Transform m_parentTransform;

	// Use this for initialization
	void Start ()
    {
        //initialize all components
        m_vikingCharacter = GetComponentInChildren<VikingController>().gameObject;
        m_valkyrieCharacter = GetComponentInChildren<ValkyrieController>().gameObject;
        m_playerData = GetComponent<PlayerData>();
        m_parentTransform = this.gameObject.transform;

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

    // Switch to Viking
    public void SwitchToViking()
    {

        //Activating viking, childing valkyrie to viking and deactivating valkyrie
        m_isValkyrie = false;
        m_vikingCharacter.SetActive(true);
        m_vikingCharacter.transform.parent = m_parentTransform;
        m_valkyrieCharacter.transform.parent = m_vikingCharacter.transform;
        m_valkyrieCharacter.SetActive(false);
    }

    // Switch to Valkyrie
    public void SwitchToValkyrie()
    {

        //Activating valkyrie, childing viking to valkyrie and deactivating viking
        m_isValkyrie = true;
        m_valkyrieCharacter.SetActive(true);
        m_valkyrieCharacter.transform.parent = m_parentTransform;
        m_vikingCharacter.transform.parent = m_valkyrieCharacter.transform;
        m_vikingCharacter.SetActive(false);
    }

}
