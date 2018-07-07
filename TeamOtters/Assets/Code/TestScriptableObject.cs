using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Scriptable Object", menuName = "Test Scriptable Object")]
public class TestScriptableObject : ScriptableObject {


    public class PlayerMovement
    {
        public float m_speed;
    }


	
    private void Whatever ()
    {
        Debug.Log("LOL");
    }
	
}
