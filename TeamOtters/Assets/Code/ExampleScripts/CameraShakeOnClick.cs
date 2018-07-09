using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraShakeOnClick : MonoBehaviour {

    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        }		
	}
}
