using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Level6 : MonoBehaviour
{
    [SerializeField] private AudioClip startingVoiceLine;
    [SerializeField] private AudioClip fixingVoiceLine1;
    [SerializeField] private AudioClip fixingVoiceLine2;

    [SerializeField] private float timeBetweenSpawns = 0.1f;
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private Collider2D spawnVolume;
    [SerializeField] private VolumeProfile volumeProfile;
    private void Start()
    {
        AudioSystem.instance.AddVoiceLineToQueue(startingVoiceLine);
      
        Player p =FindAnyObjectByType<Player>(); 
        p.canMove = true;
        p.canDash = true;
    }

    public void BugEncountered()
    {
        UnityEvent ue = new UnityEvent();
        ue.AddListener(StartMayhem);
        AudioSystem.instance.AddVoiceLineToQueue(fixingVoiceLine1, ue);
        
        UnityEvent ue2 = new UnityEvent();
        ue2.AddListener(Crash);
        AudioSystem.instance.AddVoiceLineToQueue(fixingVoiceLine2, ue2);
    }

    private void StartMayhem()
    {
        StartCoroutine(LagRoutine());
        StartCoroutine(MayhemRoutine());
    }

    private IEnumerator MayhemRoutine()
    {
        FindAnyObjectByType<Volume>().profile = volumeProfile;
        while (true)
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator LagRoutine()
    {
        for (int i = 1; i < 28; i++)
        {
            Application.targetFrameRate = 240 / i;
            yield return new WaitForSeconds(0.7f);
        }
        
    }

    private void SpawnRandomObject()
    {
        GameObject randomGameObject = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

        Bounds bounds = spawnVolume.bounds;
        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);


        GameObject newHazard = Instantiate(randomGameObject);
        newHazard.transform.position = bounds.center + new Vector3(offsetX, offsetY, 0);
    }
    private void Crash()
    {  
        MessageWindow.ShowStackOverflowError();
        LoadManager.Instance.Data.Checkpoint = 7;
        LoadManager.Instance.SaveData();
        LoadManager.Instance.Quit();
    }
}
