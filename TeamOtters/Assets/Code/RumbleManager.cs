using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class RumbleManager : MonoBehaviour {

    internal XInputDotNetPure.PlayerIndex[] m_controllerIndex = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
    public PlayerData[] m_players;
    internal bool transformationRumbleComplete;

    [Header("Transformation Camera Shake Settings")]
    public float transform_magnitude = 4f;
    public float transform_roughness = 4.5f;
    public float transform_fadeInTime = 2.5f;
    public float transform_fadeOutTime = 3f;

    [Header("Ball Hit Camera Shake Settings")]
    public float hitReact_magnitude;
    public float hitReact_roughness;
    public float hitReact_fadeInTime;
    public float hitReact_fadeOutTime;

    // Player stunned rumble
    // Player transform during phase 2 - small shake and vibration
    // Pickup vibrates

    public bool transformShakeComplete;
    public bool hitReactShakeComplete;

    // Use this for initialization
    void Start () {
        transformShakeComplete = false;
        hitReactShakeComplete  = false;
    }

    private void Update()
    {
        // Detect end of camera shakes
        if (transformShakeComplete != true)
        {
            foreach (CameraShakeInstance camShakeInstance in CameraShaker.Instance.ShakeInstances)
            {
                if (camShakeInstance.CurrentState == CameraShakeState.Inactive && camShakeInstance.DeleteOnInactive)
                {
                    // When camera shake ends, stop the rumble
                    TransformRumbleStop();

                    Debug.Log("Transform Camera Shake Complete");

                    transformShakeComplete = true;
                }
            }
        }

        if (hitReactShakeComplete != true)
        { 
            foreach (CameraShakeInstance camShakeInstance in CameraShaker.Instance.ShakeInstances)
            {
                if (camShakeInstance.CurrentState == CameraShakeState.Inactive && camShakeInstance.DeleteOnInactive)
                {
                    // When camera shake ends, stop the rumble
                    TransformRumbleStop();

                    Debug.Log("Hit React Camera Shake Complete");

                    hitReactShakeComplete = true;
                }
            }
        }
    }

    // Stop all controller vibrations on quitting the game
    private void OnApplicationQuit()
    {
        for (int i = 0; i < m_controllerIndex.Length; i++)
        {
            GamePad.SetVibration(m_controllerIndex[i], 0f, 0f);
        }
    }

    void TransformRumbleStart()
    {
        for (int i = 0; i < m_controllerIndex.Length; i++)
        {
            //Debug.Log("StartingRumble");
            GamePad.SetVibration(m_controllerIndex[i], 1f, 1f);
        }        
    }

    void TransformRumbleStop()
    {
        for (int i = 0; i < m_controllerIndex.Length; i++)
        {
            //Debug.Log("StoppingRumble");
            GamePad.SetVibration(m_controllerIndex[i], 0f, 0f);
        }        
    }    

    public void TransformShakeStart()
    {
        transformShakeComplete = false;
        CameraShaker.Instance.ShakeOnce(transform_magnitude,transform_roughness,transform_fadeInTime,transform_fadeOutTime);
       
        TransformRumbleStart();

        //Debug.Log("Starting Transform Camera Shake & Rumble");
    }

    public void HitReactShakeStart()
    {
        hitReactShakeComplete = false;
        CameraShaker.Instance.ShakeOnce(hitReact_magnitude, hitReact_roughness, hitReact_fadeInTime, hitReact_fadeOutTime);

        //Debug.Log("Starting Hit Camera Shake & Rumble");
    }
}
