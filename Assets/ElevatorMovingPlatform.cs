using System;
using DG.Tweening;
using UnityEngine;

public class ElevatorMovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform EndPoint;

    [SerializeField] private float MoveSpeed;
    
    private Vector3 _direction;
    
    private void Start()
    {
        //transform.position = SpawnPoint.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, EndPoint.position) <= MoveSpeed * Time.deltaTime)
        {
            transform.position = SpawnPoint.position;
        }
        else
        {
            var direction = EndPoint.position - transform.position;
            transform.position = transform.position + (direction.normalized * MoveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(SpawnPoint.position, EndPoint.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(SpawnPoint.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(EndPoint.position, 0.5f);
    }
}
