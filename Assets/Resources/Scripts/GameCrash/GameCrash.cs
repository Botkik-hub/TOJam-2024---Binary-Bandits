using System.Collections.Generic;
using UnityEngine;
using WBG;

public class GameCrash : MonoBehaviour
{
    private float _timeLeftToCrash;

    [SerializeField] private float StartCrashTime;

    private BugBehaviour _bug;
    private List<ICrashEffect> _effects = new List<ICrashEffect>();

    public void SetActiveBug(BugBehaviour bug)
    {
        _bug = bug;
    }

    public void AddCrashEffect(ICrashEffect effect)
    {
        _effects.Add(effect);
    }

    private void Start()
    {
        _timeLeftToCrash = StartCrashTime;
        AddCrashEffect(new FrameRateEffect());
        AddCrashEffect(new CameraEffect(UnityEngine.Camera.main.gameObject));
        var ds = new DataSaver();
        
        if (ds.TryLoad(out var data))
        {
            print("GameLoaded");
            print(data.Checkpoint);
        }
        else
        {
            print("CouldNotLoadGame");
        }
    }

    private void Update()
    {
        // if (!_bug.IsActive)
        //     return;

        _timeLeftToCrash -= Time.deltaTime;
        if (_timeLeftToCrash <= 0)
        {
            SaveAndCrash();
            return;
        }

        float progressToCrash = 1 - _timeLeftToCrash / StartCrashTime;
        foreach (var effect in _effects)
        {
            effect.SetProgress(progressToCrash);
        }
    }

    private void SaveAndCrash()
    {
        ShowErrorMessage();
        SaveData();
        CloseGame();
    }

    private void ShowErrorMessage()
    {
        MessageWindow.ShowDirectXError();
    }

    private void SaveData()
    {
        DataSaver saver = new DataSaver();
        DataSaver.Data data = new DataSaver.Data
        {
            Checkpoint = 1
        };
        saver.Save(data);
    }

    private void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif       
    }
}

public interface ICrashEffect
{
    void SetProgress(float progress);
}