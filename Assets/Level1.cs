using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WBG;

public class Level1 : MonoBehaviour
{
    [SerializeField] private List<Button> colorButtons;
    [SerializeField] private Color BuggedColor = Color.green;
    [SerializeField] private AudioClip voiceLine1;
    [SerializeField] private AudioClip voiceLine2;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = gameObject.GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        FindObjectOfType<ColorChanger>().SetColor(new Color(1,1,1,1));
        FindObjectOfType<PauseMenuManager>().SetCanPause(false);
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine1);
    }

    private void OnEnable()
    {
        foreach (var button in colorButtons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button.GetComponent<Image>().color));
        }
    }

    private void OnDisable()
    {
        foreach (var button in colorButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClicked(Color color)
    {
        AudioSystem.instance.AddVoiceLineToQueue(voiceLine2);
        LoadManager.Instance.Data.Settings.Color = color;
        FindObjectOfType<ColorChanger>().SetColor(BuggedColor);
        _canvas.gameObject.SetActive(false);
        FindAnyObjectByType<Player>().canMove = true;
        FindObjectOfType<PauseMenuManager>().SetCanPause(true);
        LoadManager.Instance.Data.Stats.StartTime = DateTime.Now;
    }

    public void OnFinalTrigger()
    {
        //todo play voice line, stop the player and wait for the end of voice here
        CrashGame();
    }

    public void CrashGame()
    {
        MessageWindow.ShowRestartMessage();
        LoadManager.Instance.Data.Checkpoint = 2;
        LoadManager.Instance.SaveData();
        LoadManager.Instance.Quit();
    }
}
