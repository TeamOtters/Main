using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraBehaviourManager : MonoBehaviour {

    [SerializeField]
    private bool m_valkyrieRaceState;

    public float m_panSpeed = 0.1f;
    public GameObject m_endPanTarget;  
   

	// Use this for initialization
	void Start () {
		
	}

    public void SetRaceState (bool enable)
    {
        m_valkyrieRaceState = enable;
    }
	
	// Update is called once per frame
	void LateUpdate () {
		

        if(m_valkyrieRaceState)
        {
            Vector3 pos = Camera.main.transform.position;
            Vector3 target = m_endPanTarget.transform.position;
            Vector3 move = new Vector3(0, target.y * m_panSpeed * Time.deltaTime, 0);
           
            transform.Translate(move, Space.World);

            //Stop Camera pan when we reached goal
            if (Mathf.Clamp(pos.y, target.y - 0.5f, target.y + 0.5f) == pos.y)
                m_valkyrieRaceState = false; 
           
        }
	}
}
