using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Level3 : MonoBehaviour
{
    [SerializeField] private AudioClip voiceLine1;
    [SerializeField] private AudioClip tooManyDeathVoiceLine2;

    
    private int _deathNumber;
    
    [SerializeField] private int DeathBeforeRestart = 3;
    
    private void Start()
    {
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine1);
        FindAnyObjectByType<Player>().canMove = true;
        FindAnyObjectByType<Player>().canDash = false;
    }

    private IEnumerator ImpossibleCountdown()
    {
        yield return new WaitForSeconds(30);
        TooManyDeathsVoiceLine();
    }


    public void OnDeath()
    {
        _deathNumber++;
        if (_deathNumber == 1)
        {
            StartCoroutine(ImpossibleCountdown());
        }
        if (_deathNumber >= DeathBeforeRestart)
        {
            TooManyDeathsVoiceLine();
        }
    }

    private void TooManyDeathsVoiceLine()
    {
        UnityEvent ue = new UnityEvent();
        ue.AddListener(CrashAndRestart);
        AudioSystem.instance.AddVoiceLineToQueue(tooManyDeathVoiceLine2, ue);
    }
    private void CrashAndRestart()
    {
        MessageWindow.ShowRestartMessage();
        LoadManager.Instance.Data.Checkpoint = 4;
        LoadManager.Instance.SaveData();
        LoadManager.Instance.Quit();
    }
}