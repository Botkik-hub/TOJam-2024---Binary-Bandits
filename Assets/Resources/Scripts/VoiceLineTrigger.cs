using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoiceLineTrigger : MonoBehaviour
{
    private bool hasBeenPlayed = false;
    [SerializeField] private AudioClip voiceLine;
    [SerializeField] private UnityEvent postVoiceEvent;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasBeenPlayed)
        {
            if(voiceLine == null)
                Debug.LogWarning("NO VOICE LINE SET FOR THIS TRIGGER");
            else
            {
                AudioSystem.instance.AddVoiceLineToQueue(voiceLine, postVoiceEvent);
                hasBeenPlayed = true;
            }
        }
    }
}
