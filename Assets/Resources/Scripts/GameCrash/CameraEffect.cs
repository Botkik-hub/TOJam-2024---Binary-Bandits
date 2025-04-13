using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffect : ICrashEffect
{
    private Volume _volume;
    private ChromaticAberration _aberration;

    public CameraEffect(GameObject camera)
    {
        bool t = camera.GetComponent<Volume>().profile.TryGet<ChromaticAberration>(out _aberration); 
        
    }
    
    public void SetProgress(float progress)
    {
        _aberration.intensity.value = Mathf.Lerp(0.4f, 1, progress);
    }
}