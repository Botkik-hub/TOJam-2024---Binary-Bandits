using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level7 : MonoBehaviour
{
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject optionsContent;
    [SerializeField] private Button lieButton;
    [SerializeField] private Button truthButton;
    [Space]
    [SerializeField] private GameObject gamePausedTextGameObject;
    [SerializeField] private GameObject stasMessageBox;
    [SerializeField] private float stasMessageBoxDisplayDuration;
    [SerializeField] private float stasMessageBoxFadeDuration;
    [Space]
    [SerializeField] private AudioClip levelStartAudioClip;
    [SerializeField] private AudioClip lieAudioClip;
    [SerializeField] private AudioClip truthAudioClip;
    [SerializeField] private AudioClip glitchIntoWallClip;

    [SerializeField] private VolumeProfile volumeProfile;

    private Player player;
    private PauseMenuManager pauseMenuManager;

    void Awake()
    {
        lieButton.onClick.AddListener(SelectLie);
        truthButton.onClick.AddListener(SelectTruth);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();

        pauseMenuManager.SetCanPause(false);

        stasMessageBox.SetActive(true);
        stasMessageBox.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0,0,false);

        gamePausedTextGameObject.SetActive(true);

        UnityEvent ue = new UnityEvent();
        ue.AddListener(DisplayOptions);

        AudioSystem.instance.AddVoiceLineToQueue(levelStartAudioClip, ue);
    }

    void SelectLie()
    {
        AudioSystem.instance.AddVoiceLineToQueue(lieAudioClip);

        UnityEvent ue = new UnityEvent();
        ue.AddListener(HideCanvas);
        HideOptions();
        StartCoroutine(ToggleStasMessageBox());
        AudioSystem.instance.AddVoiceLineToQueue(truthAudioClip, ue);
    }

    void SelectTruth()
    {
        UnityEvent ue = new UnityEvent();
        ue.AddListener(HideCanvas);
        HideOptions();
        StartCoroutine(ToggleStasMessageBox());
        AudioSystem.instance.AddVoiceLineToQueue(truthAudioClip, ue);
        LoadManager.Instance.Data.BlameStas++;
    }

    void DisplayOptions()
    {
        gamePausedTextGameObject.SetActive(false);
        optionsContent.SetActive(true);
    }

    void HideOptions()
    {
        optionsContent.SetActive(false);
    }

    void DisplayStasMessageBox()
    {
        stasMessageBox.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(1,stasMessageBoxFadeDuration, false);
        gamePausedTextGameObject.SetActive(true);
        //stasMessageBox.SetActive(true);
    }

    void HideStasMessageBox()
    {
        stasMessageBox.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0, stasMessageBoxFadeDuration, false);
        //stasMessageBox.SetActive(false);
    }

    void HideCanvas()
    {
        HideStasMessageBox();

        lieButton.onClick.RemoveAllListeners();
        truthButton.onClick.RemoveAllListeners();

        optionsCanvas.SetActive(false);
        player.canMove = true;
        player.canDash = true;

        pauseMenuManager.SetCanPause(true);
    }

    IEnumerator ToggleStasMessageBox()
    {
        DisplayStasMessageBox();
        yield return new WaitForSeconds(stasMessageBoxDisplayDuration);
        HideStasMessageBox();
    }

    public void TurnOffHazardCollision()
    {
        FindAnyObjectByType<BugManager>().AddBug(BugType.NoCollision);
    }

    public void ChangeVolume()
    {
        //AudioSystem.instance.AddVoiceLineToQueue(glitchIntoWallClip);
        FindAnyObjectByType<Volume>().profile = volumeProfile;
    }

    public void LoadLevel8()
    {
        LoadManager.Instance.Data.Checkpoint = 8;
        LoadManager.Instance.SaveData();
        AudioSystem.instance.ClearVoiceLines();
        SceneManager.LoadScene("Level8");
    }
    
}
