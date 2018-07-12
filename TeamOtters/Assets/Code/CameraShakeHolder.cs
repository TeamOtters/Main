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
    public float ballHit_magnitude = 1f;
    public float ballHit_roughness = 1f;
    public float ballHit_fadeInTime = 1f;
    public float ballHit_fadeOutTime = 1f;
    public float ballHit_vibStrength = 0.2f;

    [Header("Phase One Transform Camera Shake")]
    public float phaseOneTransform_magnitude = 4f;
    public float phaseOneTransform_roughness = 4.5f;
    public float phaseOneTransform_fadeInTime = 2.5f;
    public float phaseOneTransform_fadeOutTime = 3f;
    public float phaseOneTransform_vibStrength = 1f;

    [Header("Phase Two Transform Camera Shake")]
    public float phaseTwoTransform_magnitude = 4f;
    public float phaseTwoTransform_roughness = 4.5f;
    public float phaseTwoTransform_fadeInTime = 2.5f;
    public float phaseTwoTransform_fadeOutTime = 3f;
    public float phaseTwoTransform_vibStrength = 0.5f;

    [Header("Viking Respawn Camera Shake")]
    public float respawn_magnitude = 1f;
    public float respawn_roughness = 1f;
    public float respawn_fadeInTime = 1f;
    public float respawn_fadeOutTime = 1f;
    public float respawn_vibStrength = 0.5f;

    [Header("Stunned Controller Rumble Settings")]
    public float stunned_vibStrength = 0.5f;

    [Header("Pickup Controller Rumble Settings")]
    public float pickup_vibStrength = 0.3f;

    [Header("Hit Platform Rumble Settings")]
    public float platformHit_vibStrength = 0.2f;

    [Header("Grabby Hands Rumble Settings")]
    public float grabby_vibStrength = 0.1f; // should increase based on closeness to viking

}



