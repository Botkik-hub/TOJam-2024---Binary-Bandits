using System.IO;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private DataSaver.Data _data;
    public DataSaver.Data Data => _data;

    private bool _isLoaded = false;
    public bool IsLoaded => _isLoaded;

    private bool _fileExist = false;
    public bool FileExist => _fileExist;
    
    private static LoadManager _instance;


    public static LoadManager Instance => GetInstance();

    private static LoadManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject("Loader");
            go.AddComponent<LoadManager>();
        }

        return _instance;
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        _instance = this;
        
        DataSaver dataSaver = new DataSaver();

        _fileExist = File.Exists(DataSaver.SavePath + "\\" + DataSaver.SaveFileName);

        if (_fileExist && dataSaver.TryLoad(out _data))
        {
            _isLoaded = true;
        }
        else
        {
            _data = new DataSaver.Data();
            _isLoaded = false;
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif    
    }

    public void SaveData()
    {
        DataSaver saver = new DataSaver();
        saver.Save(_data);
    }
}