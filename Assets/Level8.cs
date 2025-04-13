using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Level8 : MonoBehaviour
{
    [Header("Options Menu")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject optionsContent;
    [SerializeField] private Button narratorButton;
    [SerializeField] private Button stasButton;
    [Space]
    [SerializeField] private GameObject gamePausedTextGameObject;
    [SerializeField] private GameObject stasMessageBox;
    [SerializeField] private float stasMessageBoxDisplayDuration;
    [SerializeField] private float stasMessageBoxFadeDuration;
    [Space]
    [SerializeField] private AudioClip levelStartAudioClip;
    [SerializeField] private AudioClip blameNarratorAudioClip;
    [SerializeField] private AudioClip blameStasAudioClip;
    [SerializeField] private AudioClip blameStasAgainAudioClip;
    [SerializeField] private AudioClip blameStasAgainAudioClip2;
    [SerializeField] private AudioClip blameStasAgainAudioClip3;
    [SerializeField] private AudioClip blameStasAgainAudioClip4;


    private Player player;
    private PauseMenuManager pauseMenuManager;

    void Awake()
    {
        narratorButton.onClick.AddListener(SelectNarrator);
        stasButton.onClick.AddListener(SelectStas);
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

    void SelectNarrator()
    {
        HideOptions();
        StartCoroutine(ToggleStasMessageBox());
        UnityEvent ue = new UnityEvent();
        ue.AddListener(EndGame);
        AudioSystem.instance.AddVoiceLineToQueue(blameNarratorAudioClip, ue);

        // SelectTruth();
    }

    void SelectStas()
    {
        //UnityEvent ue = new UnityEvent();
       // ue.AddListener(HideCanvas);
        HideOptions();
        StartCoroutine(ToggleStasMessageBox());
        // AudioSystem.instance.AddVoiceLineToQueue(truthAudioClip, ue);
        if (LoadManager.Instance.Data.BlameStas > 0)
        {
            UnityEvent ue = new UnityEvent();
            ue.AddListener(() => MessageWindow.TextureNotFound());
            AudioSystem.instance.AddVoiceLineToQueue(blameStasAgainAudioClip, ue);
            ue = new UnityEvent();
            ue.AddListener(() => MessageWindow.NoControllerFound());
            AudioSystem.instance.AddVoiceLineToQueue(blameStasAgainAudioClip2, ue);
            ue = new UnityEvent();
            ue.AddListener(() =>
            {
            MessageWindow.ShowDirectXError();
            MessageWindow.ShowOrphanError();
            MessageWindow.TextureNotFound();
            });
            AudioSystem.instance.AddVoiceLineToQueue(blameStasAgainAudioClip3, ue);
            ue = new UnityEvent();
            ue.AddListener(EndGame);
            AudioSystem.instance.AddVoiceLineToQueue(blameStasAgainAudioClip4, ue);
        }
        else
        {
            UnityEvent ue = new UnityEvent();
            ue.AddListener(EndGame);
            AudioSystem.instance.AddVoiceLineToQueue(blameStasAudioClip, ue);
        }
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

       // lieButton.onClick.RemoveAllListeners();
       // truthButton.onClick.RemoveAllListeners();

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

    public void EndGame()
    {
        MessageWindow.EndOfGame();
        LoadManager.Instance.Data.Checkpoint = -1;
        LoadManager.Instance.SaveData();
        LoadManager.Instance.Quit();
        // LoadManager.Instance.Data.Checkpoint = 8;
        // LoadManager.Instance.SaveData();
        // SceneManager.LoadScene("Level8");
    }
    
}
