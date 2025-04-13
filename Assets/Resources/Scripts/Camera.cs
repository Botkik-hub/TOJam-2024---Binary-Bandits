using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private CameraMode mode;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed;


    private void FixedUpdate()
    {
        switch (mode)
        {
            case CameraMode.FollowTarget:
                transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed);
                break;
            case CameraMode.Static:
                break;

        }
    }
}

public enum CameraMode
{
        FollowTarget,
        Static
}
