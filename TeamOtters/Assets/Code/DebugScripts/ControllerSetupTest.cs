using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSetupTest : MonoBehaviour {

	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //checking so that the input for 2 controllers is handled properly on the horizontal axis
		if(Input.GetAxis("Horizontal_P1") != 0)
        {
            Debug.Log("Player 1 pressing horizontal");
        }
        if (Input.GetAxis("Horizontal_P2") != 0)
        {
            Debug.Log("Player 2 pressing horizontal");
        }
        //checking which button is the fire button
        if(Input.GetButton("Fire1_P1"))
        {
            Debug.Log("FIRE!");
        }
    }
}
