using UnityEngine;

public class FrameRateEffect : ICrashEffect
{
    private const int MaxFrameRate = 120;
    private const int MinFrameRate = 15;
    public void SetProgress(float progress)
    {
        
        float frameRate = Mathf.Lerp(MaxFrameRate, MinFrameRate, progress);
        Application.targetFrameRate = (int)frameRate;
    }
}