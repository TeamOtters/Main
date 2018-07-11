using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraBehaviourManager : MonoBehaviour {

    [SerializeField]
    private bool m_valkyrieRaceState;

    public float m_panSpeedY = 0.1f;
    public float m_panSpeedXMin = 0.1f;
    public float m_panSpeedXMax = 0.1f;
    public GameObject m_endPanTarget;
    public float m_panXAmount;

    private float targetX;

    
    /* Rotation stuff, let's deal with this later
    public float targetYRotation = 5f;
    public float rotationSpeed = 1f;
    */

    // Use this for initialization
    void Start ()
    {
        targetX = m_panXAmount;
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
            Vector3 rot = Camera.main.transform.rotation.eulerAngles;
            Vector3 target = m_endPanTarget.transform.position;
            


            float maxDistance = m_panXAmount * 2;

            float percentageOfMax = Mathf.Abs(pos.x-targetX) / maxDistance;
            // clamp the value to 0-1 so we don't have to do a comparison
            percentageOfMax = Mathf.Clamp01(percentageOfMax);
            // if you were using lerp to change the speed...

            float dynamicSpeed;
            dynamicSpeed = Mathf.Lerp(m_panSpeedXMax, m_panSpeedXMin, percentageOfMax);

            dynamicSpeed = Mathf.SmoothStep(m_panSpeedXMin, m_panSpeedXMax, percentageOfMax);

            Vector3 move = new Vector3(targetX * dynamicSpeed, target.y * m_panSpeedY * Time.deltaTime, 0);
            Debug.Log(percentageOfMax);

            transform.Translate(move, Space.World);

            //Vector3 move = new Vector3(0f * m_panSpeedX, target.y * m_panSpeedY * Time.deltaTime, 0);




            /* Rotation stuff, let's deal with this later
            float step = rotationSpeed * Time.deltaTime;
            Vector3 targetRotation = new Vector3(0f,Time.deltaTime * rotationSpeed, 0f);

            Camera.main.transform.Rotate(targetRotation, Space.Self);

           
            if (rot.y == targetYRotation)
                targetYRotation = -targetYRotation;
            */


            //Set the new target x value for moving from left to right
            if (Mathf.Clamp(pos.x, targetX - 0.5f, targetX + 0.5f) == pos.x)
                targetX = -targetX;

            //Stop Camera pan when we reached goal
            if (Mathf.Clamp(pos.y, target.y - 0.5f, target.y + 0.5f) == pos.y)
                m_valkyrieRaceState = false; 
           
        }
	}
}
