using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioEvents
{
    public static string m_forestAmbience = "Ambience/Forest";
    public static string m_heavenAmbience = "Ambience/Heaven";
    public static string m_playerJump = "Player/Jump";
    public static string m_playerWalk = "Player/Walk";
    public static string m_playerFire = "Player/Fire";
    public static string m_playerRetract = "Player/Retract";
    public static string m_playerFlyIdle = "Player/FlyIdle";
    public static string m_playerIdentitySwitch = "Player/IdentitySwitch";
    public static string m_eggCrack = "Egg/Crack";
    public static string m_eggCrushed = "Egg/Crush";
    public static string m_eggBounce = "Egg/Bounce";
    public static string m_dragon = "Cinematic/Dragon";
    public static string m_uiGoScreen = "UI/GoScreen";
    public static string m_uiSelect = "UI/Select";
    public static string m_uiResultScreen = "UI/ResultScreen";
    public static string m_scorePickUps = "Score/PickUps";

}

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;

    public static AudioManager Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject("AudioManager").AddComponent<AudioManager>()); }
    }

    // Use this for initialization
    void Start () {

        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbience, gameObject);
        Debug.Log("Ambience");
    }

    public void StartAmbience ()
    { }

    public void PlayerJumpSound()
    { Debug.Log("JumpS"); }
    public void PlayerLandSound()
    { Debug.Log("Lands"); }
    public void PlayerThrowAxeSound()
    { Debug.Log("ThrowAxeS"); }
    public void PlayerRetractAxeSound()
    {Debug.Log("RetractS");}
    public void PlayerAxeHitSound()
    { Debug.Log("AxeHitSound"); }
    public void PlayerGotHitSound()
    { Debug.Log("PlayerGotHiSound"); }
    public void PlayerFlapSound()
    { Debug.Log("FlapS"); }
    public void PlayerDiveSound()
    { Debug.Log("DiveS"); }
    public void PlayerFlyIdleSound(bool enable)
    {
        if (enable)
            Debug.Log("FlyIdleS");
        else
            Debug.Log("StopFlyIdleS");
    }
    public void PlayerIdentitySwitchSound()
    { Debug.Log("SwitchSou"); }
    public void PhaseDragonSound ()
    { Debug.Log("Dragons"); }
    public void EggBounceSound()
    { Debug.Log("EggBounceS"); }
    public void EggCrackSound()
    { Debug.Log("EggCrackSou"); }	
    public void EggCrushedSound()
    { Debug.Log("EggCrushedSou"); }
    public void EggTakesDamageSound()
    { Debug.Log("EggDamageS"); }
    public void ScoreItemSound()
    { Debug.Log("ScoreItemSou"); }

}
