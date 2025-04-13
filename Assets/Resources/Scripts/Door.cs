using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float doorSpeed;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    private bool hasReachedTarget;
    private Vector2 targetPos;

    void Start()
    {
        targetPos = startPos.position;
    }

    public void ToggleDoor(bool toggle)
    {
        if (toggle)
        {
            StartMovingTarget(endPos);
        }
        else
        {
            StartMovingTarget(startPos);
        }
    }

    void StartMovingTarget(Transform target)
    {
        hasReachedTarget = false;
        targetPos = target.position;
    }

    void MoveToTarget()
    {
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * doorSpeed);

        if (Vector2.Distance(transform.position, targetPos) < 0.01f)
        {
            hasReachedTarget = true;
        }
    }

    void Update()
    {
        if (!hasReachedTarget)
        {
            MoveToTarget();
        }
    }

    void OnDrawGizmos()
    {
        if (startPos != null && endPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPos.position, 0.2f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPos.position, 0.2f);
        }
    }
}