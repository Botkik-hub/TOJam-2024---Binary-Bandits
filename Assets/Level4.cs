using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WBG;

public class Level4 : MonoBehaviour
{
    [SerializeField] private AudioClip voiceLine1;
    
    

    private void Start()
    {
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine1);
        FindAnyObjectByType<Player>().canMove = true;
        FindAnyObjectByType<Player>().canDash = true;
    }


    

    public void LoadLevel5()
    {

        LoadManager.Instance.Data.Checkpoint = 5;
        LoadManager.Instance.SaveData();
        AudioSystem.instance.ClearVoiceLines();
        SceneManager.LoadScene("Level5");
    }
}