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
    public static string m_playerIdentitySwitch = "Player/Transform";
    public static string m_playerLand = "Player/Land";
    public static string m_playerHit = "Player/Hit";
    public static string m_playerVOAction = "Player/VO/Action";
    public static string m_axeHit = "Axe/Hit";
    public static string m_eggCrack = "Egg/Crack";
    public static string m_eggCrushed = "Egg/Crush";
    public static string m_eggBounce = "Egg/Bounce";
    public static string m_dragon = "RTF/Dragon";
    public static string m_uiGoScreen = "UI/GoScreen";
    public static string m_uiSelect = "UI/Select";
    public static string m_uiResultScreen = "UI/ResultScreen";
    public static string m_scorePickUps = "Score/PickUps";

    public static string m_musicEvent = "Music/Game";
    public static string m_musicSwitchIdle = "Idle";
    public static string m_musicSwitchDragon = "DragonPhase";
    public static string m_musicSwitchEnd = "End";


}

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    public static AudioManager Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
       get { return instance ?? (instance = new GameObject("AudioManager").AddComponent<AudioManager>()); }
    }

    // Use this for initialization
    void Start () {

        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbience, gameObject);

        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_musicEvent, gameObject);
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_musicEvent, Fabric.EventAction.SetSwitch, AudioEvents.m_musicSwitchIdle, gameObject);
    }

    public void SwitchToHeavenAmb ()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbience, Fabric.EventAction.StopSound, gameObject);
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_heavenAmbience, gameObject);
    }
    public void SwitchToForestAmb()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_heavenAmbience, Fabric.EventAction.StopSound, gameObject);
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbience, gameObject);
    }


    public void PlayerJumpSound()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerJump, gameObject); }
    public void PlayerLandSound()
    {Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerLand, gameObject); }
    public void PlayerThrowAxeSound()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerFire, gameObject); }
    public void PlayerRetractAxeSound()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerRetract, gameObject); }
    public void PlayerAxeHitSound()
    {  Fabric.EventManager.Instance.PostEvent(AudioEvents.m_axeHit, gameObject); }
    public void PlayerGotHitSound()
    {  Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerHit, gameObject); }
    public void PlayerFlapSound()
    { PlayerJumpSound(); }
    public void PlayerDiveSound()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerVOAction, gameObject); }
    public void PlayerFlyIdleSound(bool enable)
    {
        if (enable)
            Debug.Log("FlyIdleS");
        else
            Debug.Log("StopFlyIdleS");
    }
    public void PlayerIdentitySwitchSound()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_playerIdentitySwitch, gameObject); }

    public void PhaseDragonSound ()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_dragon, gameObject);
        Invoke("SwitchToHeavenAmb", 25f);
       
    }
    public void PhaseOne()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_musicEvent, Fabric.EventAction.SetSwitch, AudioEvents.m_musicSwitchIdle, gameObject);
    }
    public void PhaseEnd ()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_musicEvent, Fabric.EventAction.SetSwitch, AudioEvents.m_musicSwitchEnd, gameObject);
    }
    public void EggBounceSound()
    { Debug.Log("EggBounceS"); Fabric.EventManager.Instance.PostEvent(AudioEvents.m_eggBounce, gameObject); }
    public void EggCrackSound()
    { Debug.Log("EggCrackSou"); Fabric.EventManager.Instance.PostEvent(AudioEvents.m_eggCrack, gameObject); }	
    public void EggCrushedSound()
    {
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_eggCrushed, gameObject);
        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_musicEvent, Fabric.EventAction.SetSwitch, AudioEvents.m_musicSwitchDragon, gameObject);
    }
    public void EggTakesDamageSound()
    { Debug.Log("EggDamageS"); }
    public void ScoreItemSound()
    { Debug.Log("ScoreItemSou"); Fabric.EventManager.Instance.PostEvent(AudioEvents.m_scorePickUps, gameObject); }

    public void UIStartScreen()
    { Fabric.EventManager.Instance.PostEvent(AudioEvents.m_uiGoScreen, gameObject); }
    

}
