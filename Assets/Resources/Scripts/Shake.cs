using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private Vector3 startPos;

    [SerializeField] private float shakeStrength;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = startPos;

        newPos.x += Random.Range(-shakeStrength, shakeStrength);
        newPos.y += Random.Range(-shakeStrength, shakeStrength);
        
        transform.position = newPos;
    }
}
