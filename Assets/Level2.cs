using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level2 : MonoBehaviour
{
    
    [SerializeField] private AudioClip voiceLine1;

    private void Start()
    {
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine1);
        FindAnyObjectByType<Player>().canMove = true;
    }

    public void LoadLevel3()
    {
        LoadManager.Instance.Data.Checkpoint = 3;
        LoadManager.Instance.SaveData();
        AudioSystem.instance.ClearVoiceLines();
        SceneManager.LoadScene("Level3");
    }

}