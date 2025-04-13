using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private void Start()
    {
        if (!LoadManager.Instance.FileExist)
        {
            SceneManager.LoadScene("Level0");
            return;
        }
        
        if (LoadManager.Instance.IsLoaded)
        {
            int checkpoint = LoadManager.Instance.Data.Checkpoint;
            if (checkpoint > 0 && checkpoint <= 8)
            {
                SceneManager.LoadScene($"level{LoadManager.Instance.Data.Checkpoint}");
            }
            else
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            canvas.SetActive(true);
        } 
    }

    public void Reset()
    {
        MessageWindow.ShowResetingMessage();
        File.Delete(DataSaver.SavePath + "\\" +DataSaver.SaveFileName);
        Exit();
    }

    public void Exit()
    {
        LoadManager.Instance.Quit();
    }
    
}
