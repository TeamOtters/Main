using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unity101 : MonoBehaviour {

    private Unity1010PublicObjects m_publicObjectScript;
    private float m_thisIsAFloat; 

	// Use this for initialization
	void Start () {

        m_publicObjectScript = (Unity1010PublicObjects)FindObjectOfType(typeof(Unity1010PublicObjects));
        m_thisIsAFloat = 5f;

      
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HEJ OTTERS");
           
        }

        float _thisIsMyfloat = 5f;
        m_publicObjectScript.SetScore(_thisIsMyfloat);

    }
}
