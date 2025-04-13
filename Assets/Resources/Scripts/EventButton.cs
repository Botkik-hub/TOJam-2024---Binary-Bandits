using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventButton : MonoBehaviour
{
    [SerializeField] private Door[] doors;

    private float startYScale;
    [SerializeField] private float targetYScale;
    [SerializeField] private float speed;

    private int collidersOnButton = 0;
    private bool isPressedIn;

    void Start()
    {
        startYScale = transform.localScale.y;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform == transform) { return; }

        collidersOnButton++;

        //print("Entered Button, Target: " + targetYScale + ", Current: " + transform.localScale.y);
        //StopAllCoroutines();
        //StartCoroutine(ButtonScaleChange(targetYScale));
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform == transform) { return; }

        collidersOnButton--;

        //print("Exited Button, Target: " + startYScale + ", Current: " + transform.localScale.y);
        //StopAllCoroutines();
        //StartCoroutine(ButtonScaleChange(startYScale));
    }

    void Update()
    {
        if (Mathf.Abs(transform.localScale.y - targetYScale) < 0.01f)
        {
            SetIsPressedIn(true);
        }
        else
        {
            SetIsPressedIn(false);
        }
    }

    void FixedUpdate()
    {
        if (collidersOnButton > 0)
        {
            if (Mathf.Abs(transform.localScale.y - targetYScale) > 0.01f)
            {
                ChangeScaleOfButton(targetYScale);
            }
        }
        else if (Mathf.Abs(transform.localScale.y - startYScale) > 0.01f)
        {
            ChangeScaleOfButton(startYScale);
        }
    }

    void ChangeScaleOfButton(float targetScale)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        if (Mathf.Abs(transform.localScale.y - targetScale) > 0.01f)
        {
            float newYScale = Mathf.MoveTowards(transform.localScale.y, targetScale, speed);
            float scaleDifference = newYScale - initialScale.y;

            transform.localScale = new Vector3(initialScale.x, newYScale, initialScale.z);

            Vector3 scaleDirection = initialRotation * Vector3.up;

            transform.position = initialPosition + scaleDirection * (scaleDifference / 2f);
        }
    }

    void SetIsPressedIn(bool toggle)
    {
        if (isPressedIn == toggle) return;

        foreach (Door door in doors)
        {
            door.ToggleDoor(toggle);
        }

        isPressedIn = toggle;
    }

    //IEnumerator ButtonScaleChange(float targetScale)
    //{
    //    Vector3 initialScale = transform.localScale;
    //    Vector3 initialPosition = transform.position;
    //    Quaternion initialRotation = transform.rotation;

    //    while (Mathf.Abs(transform.localScale.y - targetScale) > 0.01f)
    //    {
    //        float newYScale = Mathf.MoveTowards(transform.localScale.y, targetScale, Time.deltaTime * speed);
    //        float scaleDifference = newYScale - initialScale.y;

    //        transform.localScale = new Vector3(initialScale.x, newYScale, initialScale.z);

    //        Vector3 scaleDirection = initialRotation * Vector3.up;

    //        transform.position = initialPosition + scaleDirection * (scaleDifference / 2f);

    //        yield return null;
    //    }
    //}

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
