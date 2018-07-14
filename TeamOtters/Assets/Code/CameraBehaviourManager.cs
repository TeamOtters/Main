using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviourManager : MonoBehaviour {

    [SerializeField]
    private bool m_valkyrieRaceState;

    public float m_panSpeedY = 0.1f;
    public GameObject m_endPanTarget;
    
    [SerializeField]
    private AnimationCurve m_speedCurveXWhileMoving;

    [SerializeField]
    private AnimationCurve m_speedCurveXWhileStatic;

    [SerializeField]
    private AnimationCurve m_intensityCurveXWhileMoving;

    [SerializeField]
    private AnimationCurve m_speedCurveY;


    public float m_panXAmountWhileMoving;
    public float m_panXAmountWhileStatic;

    private float targetXMoving;
    private float targetXStatic;
    private float startPosY;
    private float percentage;

    internal bool m_shouldSidewaysPan = false;
    private Vector3 pos;
    private Vector3 rot;
    private Vector3 target;
    private Vector3 move;

    
    /* Rotation stuff, let's deal with this later
    public float targetYRotation = 5f;
    public float rotationSpeed = 1f;
    */

    // Use this for initialization
    void Start ()
    {
        targetXMoving = m_panXAmountWhileMoving;
        targetXStatic = m_panXAmountWhileStatic;
        startPosY = Camera.main.transform.position.y;
    }

    public void SetRaceState (bool enable)
    {
        m_valkyrieRaceState = enable;
        m_shouldSidewaysPan = true;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (m_shouldSidewaysPan)
        {
            Vector3 pos = Camera.main.transform.position;
            float shakeXModifier = m_speedCurveXWhileStatic.Evaluate(pos.x);
            Vector3 move2 = new Vector3(targetXStatic * shakeXModifier, 0, 0);
            if (m_valkyrieRaceState)
            {
                percentage = (ExtensionMethods.Remap(pos.y, startPosY, target.y, 0, 1));
                target = m_endPanTarget.transform.position;
                

                float dynamicSpeedX = m_speedCurveXWhileMoving.Evaluate(pos.x);
                float intensityModifierX = m_intensityCurveXWhileMoving.Evaluate(percentage);
                float speedModifierY = m_speedCurveY.Evaluate(percentage);
                move = new Vector3(targetXMoving * dynamicSpeedX * intensityModifierX, target.y * m_panSpeedY * speedModifierY * Time.deltaTime, 0);
                /*
                if(m_speedCurveY.Evaluate(percentage) == 0.9f)
                {
                    GameController.Instance.rumbleManager.ValhallaShake();
                } // This doesnt add too much right now
                */


                //Set the new target x value for moving from left to right
                if (Mathf.Clamp(pos.x, targetXMoving - 0.05f, targetXMoving + 0.055f) == pos.x)
                    targetXMoving = -targetXMoving;

                //Stop Camera pan when we reached goal
                if (Mathf.Clamp(pos.y, target.y - 0.5f, target.y + 0.5f) == pos.y)
                    m_valkyrieRaceState = false;


                transform.Translate(move, Space.World);
            }
            else
            {
                transform.Translate(move2, Space.World);
                if (Mathf.Clamp(pos.x, targetXStatic - 0.05f, targetXStatic + 0.055f) == pos.x)
                    targetXStatic = -targetXStatic;
            }
        }
	}


}
