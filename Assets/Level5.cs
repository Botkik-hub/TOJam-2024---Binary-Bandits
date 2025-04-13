using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WBG;

public class Level5 : MonoBehaviour
{
    [SerializeField] private AudioClip voiceLine1;



    private void Start()
    {
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine1);
        FindAnyObjectByType<Player>().canMove = true;
        FindAnyObjectByType<Player>().canDash = true;
        FindAnyObjectByType<BugManager>().AddBug(BugType.LowGravity);

    }




    public void LoadLevel6()
    {
        LoadManager.Instance.Data.Checkpoint = 6;
        LoadManager.Instance.SaveData();
        AudioSystem.instance.ClearVoiceLines();
        SceneManager.LoadScene("Level6");
    }
}