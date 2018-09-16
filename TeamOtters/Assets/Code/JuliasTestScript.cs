using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuliasTestScript : MonoBehaviour {

    public bool hasPrinted = false;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hasPrinted == false)
        {
            Debug.Log("Hi everybody, this is Julia testing!");
            hasPrinted = true;
        }
        
    }
}
