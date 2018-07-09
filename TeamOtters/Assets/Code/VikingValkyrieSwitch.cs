using System.Collections;
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
        m_vikingCharacter = GetComponentInChildren(typeof(VikingController), true).gameObject;
        m_valkyrieCharacter = GetComponentInChildren(typeof(ValkyrieController), true).gameObject;
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
        if (m_vikingCharacter != null && m_valkyrieCharacter != null)
        { 
            m_vikingCharacter.SetActive(true);
            m_vikingCharacter.transform.parent = m_parentTransform;
            m_valkyrieCharacter.transform.parent = m_vikingCharacter.transform;
            m_valkyrieCharacter.SetActive(false);
        }
        else
        {
            Debug.Log("Player is missing a character child! Make sure it has both the viking and the valkyrie! Object: " + this.gameObject.name);
        }
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
