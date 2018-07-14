using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;
using UnityEngine.Events;

public class RumbleManager : MonoBehaviour
{
    private enum ShakeType {
        None,
        BallHit,
        PhaseOneTransform,
        PhaseTwoTransform,
        VikingRespawn,
        Valhalla,
        PlayerIntro,
        Stunned,
        Pickup,
        PlatformHit,
        GrabbyHands
    };

    private ShakeType m_shakeType;

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

    internal CameraShakeInstance ballHit;
    internal CameraShakeInstance phaseOneTransform;
    internal CameraShakeInstance phaseTwoTransform;
    internal CameraShakeInstance respawn;
    internal CameraShakeInstance valhalla;
    internal XInputDotNetPure.PlayerIndex[] m_controllerIndex = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

    private bool m_gameEnded;

    public UnityEvent PhaseOneTreeShakes;

    private void Update()
    {
        m_gameEnded = GameController.Instance.goalLine.GetGameOverState();

        if(m_gameEnded)
        {
            AllControllersVibrate(0f);
            SetBoolTypes(false);
        }

        // DEBUG, Q to test ball hit shake
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Should start rumbling");
            BallHitShake();
        }       


        // DEBUG, E to test rumble
        if(Input.GetKeyDown(KeyCode.E))
        {            
            PlayerStunnedVibrate(1);
        }

        // DEBUG, R to test rumble
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Should be vibrating");
            PickupVibrate(0);           
        }
    }

    IEnumerator WaitForRumble(CameraShakeInstance instance, float timeIn, float timeOut)
    {
        instance.DeleteOnInactive = false;        
        SetBoolTypes(true);
        yield return new WaitForSecondsRealtime(timeIn);

        instance.StartFadeOut(timeOut);

        yield return new WaitForSecondsRealtime(timeOut);
        instance.DeleteOnInactive = true;

        if (instance.CurrentState == CameraShakeState.Inactive)
        {
            SetBoolTypes(false);

            AllControllersVibrate(0f); // set up needed to turn individual player controllers off if its only affecting certain players
        }     
    }

    private void SetBoolTypes (bool enable)
    {
        if (m_shakeType == ShakeType.BallHit)
        {
            ballHitRumbling = enable;
            Debug.Log("Ballhit rumbling is now: " + ballHitRumbling);
        }
        else if (m_shakeType == ShakeType.PhaseOneTransform)
        {
            phaseOneTransformRumbling = enable;
            Debug.Log("PhaseOneTransform rumbling is now: " + phaseOneTransformRumbling);
        }
        else if (m_shakeType == ShakeType.PhaseTwoTransform)
        {
            phaseTwoTransformRumbling = enable;
            Debug.Log("PhaseTwoTransform rumbling is now: " + phaseTwoTransformRumbling);
        }
        else if (m_shakeType == ShakeType.VikingRespawn)
        {
            vikingRespawnRumbling = enable;
            Debug.Log("VikingRespawn rumbling is now: " + vikingRespawnRumbling);
        }
        else if (m_shakeType == ShakeType.Valhalla)
        {
            valhallaRumbling = enable;
            Debug.Log("Valhalla rumbling is now: " + valhallaRumbling);
        }
        else if (m_shakeType == ShakeType.PlayerIntro)
        {
            playerIntroRumbling = enable;
            Debug.Log("PlayerIntro rumbling is now: " + playerIntroRumbling);
        }
        else if (m_shakeType == ShakeType.Stunned)
        {
            stunnedRumbling = enable;
            Debug.Log("Stunned rumbling is now: " + stunnedRumbling);
        }
        else if (m_shakeType == ShakeType.Pickup)
        {
            pickupRumbling = enable;
            Debug.Log("Pickup rumbling is now: " + pickupRumbling);
        }
        else if (m_shakeType == ShakeType.PlatformHit)
        {
            platformHitRumbling = enable;
            Debug.Log("PlatformHit rumbling is now: " + platformHitRumbling);
        }
        else if (m_shakeType == ShakeType.GrabbyHands)
        {
            grabbyHandsRumbling = enable;
            Debug.Log("GrabbyHands rumbling is now: " + grabbyHandsRumbling);
        }
        else
        {
            Debug.Log("This shouldn't happen, no rumble shake type defined. What did you do?");
        }

    }

    IEnumerator WaitForVibrate(float time, int playerController)
    {
        SetBoolTypes(true);
        yield return new WaitForSecondsRealtime(time);

        SingleControllerVibrate(0f, playerController);
        SetBoolTypes(false);
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
            SingleControllerVibrate(vibrateStrength, i+1);
        }
    }

    void SingleControllerVibrate(float vibrateStrength, int playerIndex)
    {
        GamePad.SetVibration(m_controllerIndex[playerIndex-1], vibrateStrength, vibrateStrength);
    }

    //------------------------------------------------------------------
    // Screen shakes (affect all player controllers and shared screen)
    //------------------------------------------------------------------

    public void BallHitShake()
    {
        if (!m_gameEnded)
        {
            CameraShakeInstance instance;

            instance = ShakeSetup(ballHit, ballHit_magnitude, ballHit_roughness, ballHit_fadeInTime);

            AllControllersVibrate(phaseTwoTransform_vibStrength);

            m_shakeType = ShakeType.BallHit;

            StartCoroutine(WaitForRumble(instance, ballHit_fadeInTime, ballHit_fadeOutTime));
        }

    }

    public void PhaseOneShake()
    {
        if (!m_gameEnded)
        {
            CameraShakeInstance instance;

            instance = ShakeSetup(phaseOneTransform, phaseOneTransform_magnitude, phaseOneTransform_roughness, phaseOneTransform_fadeInTime);

            AllControllersVibrate(phaseOneTransform_vibStrength);

            StartCoroutine(WaitForRumble(instance, phaseOneTransform_fadeInTime, phaseOneTransform_fadeOutTime));

            PhaseOneTreeShakes.Invoke();
        }
    }

    public void PhaseTwoShake(int vikingPlayerIndex, int valkyriePlayerIndex)
    {
        if (!m_gameEnded)
        {
            CameraShakeInstance instance;

            instance = ShakeSetup(phaseTwoTransform, phaseTwoTransform_magnitude, phaseTwoTransform_roughness, phaseTwoTransform_fadeInTime);

            SingleControllerVibrate(phaseTwoTransform_vibStrength, vikingPlayerIndex);
            SingleControllerVibrate(phaseTwoTransform_vibStrength, valkyriePlayerIndex);

            StartCoroutine(WaitForRumble(instance, phaseTwoTransform_fadeInTime, phaseTwoTransform_fadeOutTime));
        }
    }

    public void VikingRespawnShake()
    {
        if (!m_gameEnded)
        {
            CameraShakeInstance instance;

            instance = ShakeSetup(respawn, respawn_magnitude, respawn_roughness, respawn_fadeInTime);

            AllControllersVibrate(respawn_vibStrength);

            StartCoroutine(WaitForRumble(instance, respawn_fadeInTime, respawn_fadeOutTime));
        }
    }

    public void ValhallaShake()
    {
        CameraShakeInstance instance;

        instance = ShakeSetup(valhalla, valhalla_magnitude, valhalla_roughness, valhalla_fadeInTime);

        AllControllersVibrate(valhalla_vibStrength);

        StartCoroutine(WaitForRumble(instance, valhalla_fadeInTime, valhalla_fadeOutTime));        
    }

    //----------------------------------------------
    // Controller vibrations
    //----------------------------------------------

    public void PlayerIntroVibrate(int playerIndex)
    {
        if (!m_gameEnded)
        {
            SingleControllerVibrate(intro_vibStrength, playerIndex);

            StartCoroutine(WaitForVibrate(intro_vibTime, playerIndex));
        }
    }

    public void PlayerStunnedVibrate(int playerIndex)
    {
        if (!m_gameEnded)
        {
            SingleControllerVibrate(stunned_vibStrength, playerIndex);

            StartCoroutine(WaitForVibrate(stunned_vibTime, playerIndex));
        }
    }

    public void PickupVibrate(int playerIndex)
    {
        if (!m_gameEnded)
        {
            SingleControllerVibrate(pickup_vibStrength, playerIndex);

            StartCoroutine(WaitForVibrate(pickup_vibTime, playerIndex));
        }
    }

    public void PlatformHitVibrate(int playerIndex)
    {
        if (!m_gameEnded)
        {
            SingleControllerVibrate(platformHit_vibStrength, playerIndex);

            StartCoroutine(WaitForVibrate(platformHit_vibTime, playerIndex));
        }
    }

    public void GrabbyHandsVibrate(float vibrateAmount, int playerIndex)
    {
        if (!m_gameEnded)
        {
            if (vibrateAmount == 0f && GameController.Instance.phaseManager.m_players[playerIndex - 1].gameObject.GetComponent<VikingValkyrieSwitch>().m_isValkyrie)
            {
                GameController.Instance.phaseManager.m_players[playerIndex - 1].gameObject.GetComponentInChildren<ValkyrieController>().isCloseToViking = false;
            }

            SingleControllerVibrate(vibrateAmount, playerIndex);

            if (vibrateAmount > 0f)
                grabbyHandsRumbling = true;
            else
                grabbyHandsRumbling = false;
        }
    }

    private void OnApplicationQuit()
    {
        AllControllersVibrate(0f);
    }
}



