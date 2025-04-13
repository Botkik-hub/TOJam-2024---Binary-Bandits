using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;

    [SerializeField] private GameObject sfxGameObject;
    [SerializeField] private GameObject speakerIcon;

    [Space]

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource voiceLineSource;

    private Queue<VoiceLine> voiceLineQueue = new Queue<VoiceLine>();
    private List<AudioSource> activeSoundEffectAudioSources = new List<AudioSource>();
    private List<AudioSource> audioSourcesToRemoveFromActiveSFX = new List<AudioSource>();

    private VoiceLine currVoiceLine;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += ClearSounds;
        }

        speakerIcon.GetComponent<RawImage>().CrossFadeAlpha(0, 0, true);
    }

    void Update()
    {
        if (currVoiceLine != null)
        {
            currVoiceLine.Update();

            if (currVoiceLine.isRunning)
            {
                DisplaySpeaker();
            }
            else
            {
                HideSpeaker();
            }
        }

        CheckForNextVoiceLine();
    }

    #region Sound Control

    void ClearSounds(Scene scene, LoadSceneMode mode)
    {
        activeSoundEffectAudioSources.Clear();
    }

    public void PauseAllSounds()
    {
        voiceLineSource.Pause();
        musicSource.Pause();

        for (int i = 0; i < activeSoundEffectAudioSources.Count; i++)
        {
            if (activeSoundEffectAudioSources[i] != null)
            {
                activeSoundEffectAudioSources[i].Pause();
            }
            else
            {
                audioSourcesToRemoveFromActiveSFX.Add(activeSoundEffectAudioSources[i]);
            }
        }

        foreach (AudioSource src in audioSourcesToRemoveFromActiveSFX)
        {
            activeSoundEffectAudioSources.Remove(src);
        }
    }

    public void ResumeAllSounds()
    {
        voiceLineSource.UnPause();
        musicSource.UnPause();

        for (int i = 0; i < activeSoundEffectAudioSources.Count; i++)
        {
            if (activeSoundEffectAudioSources[i] != null)
            {
                activeSoundEffectAudioSources[i].UnPause();
            }
            else
            {
                audioSourcesToRemoveFromActiveSFX.Add(activeSoundEffectAudioSources[i]);
            }
        }

        foreach (AudioSource src in audioSourcesToRemoveFromActiveSFX)
        {
            activeSoundEffectAudioSources.Remove(src);
        }
    }

    public void ClearVoiceLines()
    {
        voiceLineQueue.Clear();

        musicSource.clip = null;
        voiceLineSource.clip = null;
    }

    public void ChangeVolume(VolumeCategory category, float volume)
    {
        volume = Mathf.Log10(volume) * 20;

        switch (category)
        {
            case VolumeCategory.Master:
                audioMixer.SetFloat("Master", volume);
                break;
            case VolumeCategory.Music:
                audioMixer.SetFloat("Music", volume);
                break;
            case VolumeCategory.SFX:
                audioMixer.SetFloat("SFX", volume);
                break;
            case VolumeCategory.VoiceLine:
                audioMixer.SetFloat("Voice", volume);
                break;
        }
    }
    #endregion

    #region Adding/Playing Sounds

    public void PlaySoundAtLocation(AudioClip clip, Vector2 position)
    {
        if (clip == null)
        {
            return;
        }

        AudioSource audioSource = Instantiate(sfxGameObject, position, Quaternion.identity).GetComponent<AudioSource>();

        if (audioSource == null) { return; }

        audioSource.clip = clip;

        //Set Volume

        audioSource.Play();

        float clipLength = audioSource.clip.length;
        activeSoundEffectAudioSources.Add(audioSource);
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void AddVoiceLineToQueue(AudioClip clip, UnityEvent postClipEvent = null)
    {
        VoiceLine voiceLine = new VoiceLine(clip, postClipEvent, FindObjectOfType<PauseMenuManager>());

        voiceLineQueue.Enqueue(voiceLine);

        if (currVoiceLine == null || (currVoiceLine.clipDonePlaying && currVoiceLine.eventRan))
        {
            PlayNextVoiceLine();
        }
    }

    void PlayNextVoiceLine()
    {
        VoiceLine voiceLine = voiceLineQueue.Dequeue();
        currVoiceLine = voiceLine;
        voiceLine.Run(voiceLineSource);
    }

    void CheckForNextVoiceLine()
    {
        if (currVoiceLine == null) { return; }

        if (currVoiceLine.clipDonePlaying && currVoiceLine.eventRan && voiceLineQueue.Count > 0)
        {
            PlayNextVoiceLine();
        }
    }

    #endregion


    void DisplaySpeaker()
    {
        if (speakerIcon)
        {
            if (speakerIcon.GetComponent<RawImage>().color.a < 127.5)
                speakerIcon.GetComponent<RawImage>().CrossFadeAlpha(0.5f, 0.25f, true);
        }
    }

    void HideSpeaker()
    {
        if (speakerIcon)
        {
            if (speakerIcon.GetComponent<RawImage>().color.a >= 0)
                speakerIcon.GetComponent<RawImage>().CrossFadeAlpha(0, 0.25f, true);
        }
    }
}

public enum VolumeCategory
{
    Master,
    Music,
    SFX,
    VoiceLine
}

public class VoiceLine
{
    public AudioClip voiceLineClip;
    public UnityEvent voiceLineEvent;

    private PauseMenuManager pauseMenuManager;

    public bool clipDonePlaying = false;
    public bool eventRan = false;
    public bool isRunning = false;

    private AudioSource audioSource;

    public VoiceLine(AudioClip clip, UnityEvent postClipEvent, PauseMenuManager pauseMenu)
    {
        pauseMenuManager = pauseMenu;

        voiceLineClip = clip;
        voiceLineEvent = postClipEvent;
    }

    public void Run(AudioSource source)
    {
        isRunning = true;

        audioSource = source;
        audioSource.clip = voiceLineClip;
        audioSource.Play();
    }

    private void RunEvent()
    {
        if (voiceLineEvent == null)
        {
            eventRan = true;
        }
        else
        {
            voiceLineEvent.Invoke();
            eventRan = true;
        }
    }

    public void Update()
    {
        if(audioSource && pauseMenuManager)
        {
            if (isRunning && !audioSource.isPlaying && Application.isFocused && !pauseMenuManager.IsPaused())
            {
                isRunning = false;
                clipDonePlaying = true;
                RunEvent();
            }
        }
    }
}
