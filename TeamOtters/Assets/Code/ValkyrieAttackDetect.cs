using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieAttackDetect : MonoBehaviour {
    
    private ValkyrieController m_valkyrie;

    // Use this for initialization
    void OnStart ()
    {
        m_valkyrie = GetComponentInParent<ValkyrieController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Valkyrie")
        {
            // Make other valkyrie drop pickup if carrying and hit
            other.GetComponent<ValkyrieController>().DropPickup(); //should change if we do a shield mechanic
            other.GetComponent<ValkyrieController>().GetStunned();

            Debug.Log("OMG YOU HIT A VALKYRIE!");
        }
    }
}
