using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class GoalReached : MonoBehaviour {
    public Canvas m_Results;

	// Use this for initialization
	void Start () {
        Debug.Log("I am real");
	}

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Valkyrie") && player.gameObject.GetComponent<ValkyrieController>().isCarrying == (true))
        {
            int ID = player.gameObject.GetComponent<ValkyrieController>().m_playerIndex;
            Debug.Log("PLAYER " + ID + " YOU WIN!!!");
            m_Results.gameObject.SetActive(true);

        }

        else
        {
            Debug.Log("I'm NOT a Valkyrie carrying a viking.");
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
