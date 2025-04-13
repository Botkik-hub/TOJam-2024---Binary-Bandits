using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Saw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0f, 0f, 360), 0.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
