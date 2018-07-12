using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class CameraShakeHolder : MonoBehaviour
{
    public CameraShaker ballHit;
    public CameraShaker phaseOneTransform;
    public CameraShaker phaseTwoTransform;
    public CameraShaker pickup;
    public CameraShaker hitPlatform;
    public CameraShaker grabbyHands;

    [Header("Ball Hit Camera Shake Settings")]
    public float ballHit_magnitude;
    public float ballHit_roughness;
    public float ballHit_fadeInTime;
    public float ballHit_fadeOutTime;

    [Header("Phase One Transform Camera Shake")]
    public float phaseOneTransform_magnitude = 4f;
    public float phaseOneTransform_roughness = 4.5f;
    public float phaseOneTransform_fadeInTime = 2.5f;
    public float phaseOneTransform_fadeOutTime = 3f;

    [Header("Phase Two Transform Camera Shake")]
    public float phaseTwoTransform_magnitude = 4f;
    public float phaseTwoTransform_roughness = 4.5f;
    public float phaseTwoTransform_fadeInTime = 2.5f;
    public float phaseTwoTransform_fadeOutTime = 3f;

    [Header("Stunned Controller Rumble Settings")]
    public float stunnedVibStrength = 0.5f;

    [Header("Pickup Controller Rumble Settings")]
    public float pickupVibStrength = 0.3f;

    [Header("Hit Platform Rumble Settings")]
    public float platformHitVibStrength = 0.2f;

    [Header("Grabby Hands Rumble Settings")]
    public float grabbyVibStrength = 0.1f; // should increase based on closeness to viking

}



