using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioEvents
{
    public static string m_forestAmbienceSoundEvent = "Ambience/Forest";
}

public class AudioManager : MonoBehaviour {

   

	// Use this for initialization
	void Start () {

        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbienceSoundEvent, Fabric.EventAction.StopSound, gameObject);

        Fabric.EventManager.Instance.PostEvent(AudioEvents.m_forestAmbienceSoundEvent, gameObject);
    }

    public void StartAmbience ()
    { }

    public void StartFaceTwo ()
    {

    }
    public void ProjectileSounds(bool enable)
    {
        if(enable)
        { //initiatlise throw sound
        }
         
        else
        {
          //retract sound 
        }
    }
	

}
