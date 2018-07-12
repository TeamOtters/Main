using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraBehaviourManager : MonoBehaviour {

    [SerializeField]
    private bool m_valkyrieRaceState;

    public float m_panSpeedY = 0.1f;
    public GameObject m_endPanTarget;
    
    [SerializeField]
    private AnimationCurve m_speedCurveX;
    [SerializeField]
    private AnimationCurve m_speedCurveY;


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
        /*
        m_speedCurve.AddKey(-m_panXAmount, m_panSpeedXMin);
        m_speedCurve.AddKey(0, m_panSpeedXMax);
        m_speedCurve.AddKey(m_panXAmount, m_panSpeedXMin);
        for (int i = 0; i < m_speedCurve.keys.Length; ++i)
        {
            m_speedCurve.SmoothTangents(i, 0); //zero weight means average
        }
        */
    }

    public void SetRaceState (bool enable)
    {
        m_valkyrieRaceState = enable;
    }
	
	// Update is called once per frame
	void Update () {
		

        if(m_valkyrieRaceState)
        {
            Vector3 pos = Camera.main.transform.position;
            Vector3 rot = Camera.main.transform.rotation.eulerAngles;
            Vector3 target = m_endPanTarget.transform.position;

            float relativeYPos = (target.y - pos.y)/target.y;

            /*
            float maxDistance = m_panXAmount * 2;
            
            float percentageOfMax = Mathf.Abs(pos.x-targetX) / maxDistance;
            // clamp the value to 0-1 so we don't have to do a comparison
            percentageOfMax = Mathf.Clamp01(percentageOfMax);

            float dynamicSpeed;
            dynamicSpeed = Mathf.Lerp(m_panSpeedXMax, m_panSpeedXMin, percentageOfMax);
            */

            float dynamicSpeedX = m_speedCurveX.Evaluate(pos.x);
            float speedModifierY = m_speedCurveY.Evaluate(relativeYPos);
            Vector3 move = new Vector3(targetX * dynamicSpeedX, target.y * m_panSpeedY *speedModifierY * Time.deltaTime, 0);


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
            if (Mathf.Clamp(pos.x, targetX - 0.05f, targetX + 0.055f) == pos.x)
                targetX = -targetX;

            //Stop Camera pan when we reached goal
            if (Mathf.Clamp(pos.y, target.y - 0.5f, target.y + 0.5f) == pos.y)
                m_valkyrieRaceState = false; 
           
        }
	}
}
