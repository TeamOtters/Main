using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class CameraShakeHolder : MonoBehaviour
{  
    [Header("Ball Hit Camera Shake Settings")]
    public float ballHit_magnitude = 2f;
    public float ballHit_roughness = 2f;
    public float ballHit_fadeInTime = 0.05f;
    public float ballHit_fadeOutTime = 0.2f;
    public float ballHit_vibStrength = 0.1f;

    [Header("Phase One Transform Camera Shake")]
    public float phaseOneTransform_magnitude = 4f;
    public float phaseOneTransform_roughness = 4.5f;
    public float phaseOneTransform_fadeInTime = 2.5f;
    public float phaseOneTransform_fadeOutTime = 3f;
    public float phaseOneTransform_vibStrength = 1f;

    [Header("Phase Two Transform Camera Shake")]
    public float phaseTwoTransform_magnitude = 2f;
    public float phaseTwoTransform_roughness = 3f;
    public float phaseTwoTransform_fadeInTime = 0.3f;
    public float phaseTwoTransform_fadeOutTime = 1f;
    public float phaseTwoTransform_vibStrength = 0.5f;

    [Header("Viking Respawn Camera Shake")]
    public float respawn_magnitude = 1f;
    public float respawn_roughness = 1f;
    public float respawn_fadeInTime = 0.3f;
    public float respawn_fadeOutTime = 2f;
    public float respawn_vibStrength = 0.5f;

    [Header("Reached Valhalla Camera Shake")]
    public float valhalla_magnitude = 4f;
    public float valhalla_roughness = 5f;
    public float valhalla_fadeInTime = 6f;
    public float valhalla_fadeOutTime = 8f;
    public float valhalla_vibStrength = 0.5f;

    [Header("Player Intro Rumble Settings")] // affected player only
    public float intro_vibStrength = 0.5f;
    public float intro_vibTime = 1f;

    [Header("Stunned Controller Rumble Settings")] // affected player only
    public float stunned_vibStrength = 0.5f;
    public float stunned_vibTime = 1f;

    [Header("Pickup Controller Rumble Settings")] // affected player only
    public float pickup_vibStrength = 0.3f;
    public float pickup_vibTime = 1f;

    [Header("Hit Platform Rumble Settings")] // affected player only?
    public float platformHit_vibStrength = 0.2f;
    public float platformHit_vibTime = 1f;

    internal bool ballHitRumbling;
    internal bool phaseOneTransformRumbling;
    internal bool phaseTwoTransformRumbling;
    internal bool vikingRespawnRumbling;
    internal bool playerIntroRumbling;
    internal bool stunnedRumbling;
    internal bool pickupRumbling;
    internal bool platformHitRumbling;
    internal bool grabbyHandsRumbling;
    internal bool valhallaRumbling;

    private CameraShakeInstance ballHit;
    private CameraShakeInstance phaseOneTransform;
    private CameraShakeInstance phaseTwoTransform;
    private CameraShakeInstance respawn;
    private CameraShakeInstance valhalla;
    private XInputDotNetPure.PlayerIndex[] m_controllerIndex = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

    private void Update()
    {
        /*// DEBUG, Q to test ball hit shake
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Should start rumbling");
            BallHitShake();
        }
        
        // DEBUG, W to test shake
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Should start rumbling");
            PhaseTwoShake();
        }

        // DEBUG, E to test rumble
        if(Input.GetKeyDown(KeyCode.E))
        {            
            GrabbyHandsVibrate(1, 0);
        }

        // DEBUG, R to test rumble
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Should be vibrating");
            PickupVibrate(0);           
        }*/

    }

    IEnumerator WaitForRumble(CameraShakeInstance instance, float timeIn, float timeOut, bool rumbling)
    {
        instance.DeleteOnInactive = false;
        SetToTrue(rumbling);
        Debug.Log("Rumble is now: " + rumbling);

        yield return new WaitForSecondsRealtime(timeIn);

        instance.StartFadeOut(timeOut);

        yield return new WaitForSecondsRealtime(timeOut);

        if (instance.CurrentState == CameraShakeState.Inactive)
        {
            SetToFalse(rumbling);
            Debug.Log("Rumble is now: " + rumbling); // bools arent being set inside here :( should probably fix

            AllControllersVibrate(0f); // set up needed to turn individual player controllers off if its only affecting certain players
        }     
    }

    IEnumerator WaitForVibrate(float time, bool vibrateBool, int playerController)
    {
        vibrateBool = true;
        yield return new WaitForSecondsRealtime(time);

        SingleControllerVibrate(0f, playerController); 
        vibrateBool = false;
    }

    void SetToTrue(bool myBool)
    {
        myBool = true; 
    }

    void SetToFalse(bool myBool)
    {
        myBool = true;
    }

    private CameraShakeInstance ShakeSetup(CameraShakeInstance instance, float mag, float rough, float timeIn)
    {        
        if (instance == null) //First time shake setup
            instance = CameraShaker.Instance.StartShake(mag, rough, timeIn); // Create shake
        else 
            instance.StartFadeIn(timeIn); //Start shaker instance shaking

        return instance;
    }

    void AllControllersVibrate(float vibrateStrength)
    {
        for (int i = 0; i < m_controllerIndex.Length; i++)
        {
            SingleControllerVibrate(vibrateStrength, i);
        }
    }

    void SingleControllerVibrate(float vibrateStrength, int playerIndex)
    {
        GamePad.SetVibration(m_controllerIndex[playerIndex], vibrateStrength, vibrateStrength);
    }

    //----------------------------------------------
    // Screen shakes
    //----------------------------------------------

    void BallHitShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(ballHit, ballHit_magnitude, ballHit_roughness, ballHit_fadeInTime);

        AllControllersVibrate(phaseTwoTransform_vibStrength);

        StartCoroutine(WaitForRumble(instance, ballHit_fadeInTime, ballHit_fadeOutTime, ballHitRumbling));
    }

    public void PhaseOneShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(phaseOneTransform, phaseOneTransform_magnitude, phaseOneTransform_roughness, phaseOneTransform_fadeInTime);

        AllControllersVibrate(phaseOneTransform_vibStrength);

        StartCoroutine(WaitForRumble(instance, phaseOneTransform_fadeInTime, phaseOneTransform_fadeOutTime, phaseOneTransformRumbling));
    }

    public void PhaseTwoShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(phaseTwoTransform, phaseTwoTransform_magnitude, phaseTwoTransform_roughness, phaseTwoTransform_fadeInTime);

        AllControllersVibrate(phaseTwoTransform_vibStrength);

        StartCoroutine(WaitForRumble(instance, phaseTwoTransform_fadeInTime, phaseTwoTransform_fadeOutTime, phaseTwoTransformRumbling));
    }

    public void VikingRespawnShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(respawn, respawn_magnitude, respawn_roughness, respawn_fadeInTime);

        AllControllersVibrate(respawn_vibStrength);

        StartCoroutine(WaitForRumble(instance, respawn_fadeInTime, respawn_fadeOutTime, vikingRespawnRumbling));
    }

    public void ValhallaShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(valhalla, valhalla_magnitude, valhalla_roughness, valhalla_fadeInTime);

        AllControllersVibrate(valhalla_vibStrength);

        StartCoroutine(WaitForRumble(instance, valhalla_fadeInTime, valhalla_fadeOutTime, valhallaRumbling));
    }

    //----------------------------------------------
    // Controller vibrations
    //----------------------------------------------

    public void PlayerIntroVibrate(int playerIndex)
    {
        SingleControllerVibrate(intro_vibStrength, playerIndex);

        StartCoroutine(WaitForVibrate(intro_vibTime, playerIntroRumbling, playerIndex));        // need to pass in individual indexes
    }

    public void PlayerStunnedVibrate(int playerIndex)
    {
        SingleControllerVibrate(stunned_vibStrength, playerIndex);

        StartCoroutine(WaitForVibrate(stunned_vibTime, stunnedRumbling, playerIndex));        // need to pass in individual indexes
    }

    public void PickupVibrate(int playerIndex)
    {
        SingleControllerVibrate(pickup_vibStrength, playerIndex);

        StartCoroutine(WaitForVibrate(pickup_vibTime, pickupRumbling, playerIndex));        // need to pass in individual indexes
    }

    public void PlatformHitVibrate(int playerIndex)
    {
        SingleControllerVibrate(platformHit_vibStrength, playerIndex);

        StartCoroutine(WaitForVibrate(platformHit_vibTime, platformHitRumbling, playerIndex));        // need to pass in individual indexes
    }

    public void GrabbyHandsVibrate(float vibrateAmount, int playerIndex)
    {
        SingleControllerVibrate(vibrateAmount, playerIndex);

        if (vibrateAmount > 0f)
            grabbyHandsRumbling = true;
        else
            grabbyHandsRumbling = false;
    }

}



