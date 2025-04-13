using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenuGameObject;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings")]
    [SerializeField] private GameObject settingsMenuGameObject;

    [SerializeField] private Button quitSettingsButton;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Button clearGameDataButton;

    [SerializeField] private Button[] colorButtons;

    [Header("Clear Data Window")]
    [SerializeField] private GameObject clearDataWindowGameObject;

    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;


    private bool _isPaused;
    private bool _canPause = true;

    public void SetCanPause(bool canPause)
    {
        _canPause = canPause;
    }

    void Awake()
    {
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCreditsScene);
        quitButton.onClick.AddListener(Quit);
        quitSettingsButton.onClick.AddListener(CloseSettings);

        foreach (var button in colorButtons)
        {
            button.onClick.AddListener(() => ChangeLevelColor(button.GetComponent<Image>().color));
        }

        masterSlider.onValueChanged.AddListener(value => AudioSystem.instance.ChangeVolume(VolumeCategory.Master, value));
        musicSlider.onValueChanged.AddListener(value => AudioSystem.instance.ChangeVolume(VolumeCategory.Music, value));
        sfxSlider.onValueChanged.AddListener(value => AudioSystem.instance.ChangeVolume(VolumeCategory.SFX, value));
        voiceSlider.onValueChanged.AddListener(value => AudioSystem.instance.ChangeVolume(VolumeCategory.VoiceLine, value));
        clearGameDataButton.onClick.AddListener(DisplayClearDataWindow);
        
        confirmButton.onClick.AddListener(ClearGameData);
        cancelButton.onClick.AddListener(CloseClearDataWindow);
    }

    private void Start()
    {
        LoadManager lm = LoadManager.Instance;
        DataSaver.Data.SettingsData data = lm.Data.Settings;
        masterSlider.value = data.Master;
        AudioSystem.instance.ChangeVolume(VolumeCategory.Master, data.Master);
        musicSlider.value = data.Music;
        AudioSystem.instance.ChangeVolume(VolumeCategory.Music, data.Music);
        sfxSlider.value = data.SoundEffects;
        AudioSystem.instance.ChangeVolume(VolumeCategory.SFX, data.SoundEffects);
        voiceSlider.value = data.VoiceLines;
        AudioSystem.instance.ChangeVolume(VolumeCategory.VoiceLine, data.VoiceLines);
    }

    void OnDestroy()
    {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        quitSettingsButton.onClick.RemoveAllListeners();

        foreach (var button in colorButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        voiceSlider.onValueChanged.RemoveAllListeners();
        clearGameDataButton.onClick.RemoveAllListeners();

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    public void Resume()
    {
        CloseSettings();
        CloseClearDataWindow();
        pauseMenuGameObject.SetActive(false);
        Time.timeScale = 1.0f;
        AudioSystem.instance.ResumeAllSounds();
        _isPaused = false;
    }

    public void Pause()
    {
        if(!_canPause) {return;}

        AudioSystem.instance.PauseAllSounds();
        Time.timeScale = 0.0f;
        pauseMenuGameObject.SetActive(true);
        _isPaused = true;
    }

    void Restart()
    {
        AudioSystem.instance.ClearVoiceLines();
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OpenSettings()
    {
        pauseMenuGameObject.SetActive(false);
        settingsMenuGameObject.SetActive(true);
    }

    void OpenCreditsScene()
    {
        Resume();
        SceneManager.LoadScene("Level0");
    }

    void CloseSettings()
    {
        settingsMenuGameObject.SetActive(false);
        pauseMenuGameObject.SetActive(true);
        var data = LoadManager.Instance.Data.Settings;
        data.Master = masterSlider.value;
        data.Music = musicSlider.value;
        data.SoundEffects = sfxSlider.value;
        data.VoiceLines = voiceSlider.value;
        LoadManager.Instance.SaveData();
    }

    void DisplayClearDataWindow()
    {
        clearDataWindowGameObject.SetActive(true);
    }

    void CloseClearDataWindow()
    {
        clearDataWindowGameObject.SetActive(false);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif     
    }

    void ClearGameData()
    {
        AudioSystem.instance.ClearVoiceLines();
        Resume();
        LoadManager.Instance.Data.Checkpoint = 0;
        LoadManager.Instance.SaveData();
        SceneManager.LoadScene("Level0");
    }

    private void ChangeLevelColor(Color color)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
        {
            return;
        }

        LoadManager.Instance.Data.Settings.Color = color;
        FindObjectOfType<ColorChanger>().SetColor(color);
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

}
