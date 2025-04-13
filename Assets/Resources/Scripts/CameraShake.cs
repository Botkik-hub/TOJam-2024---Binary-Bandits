using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private bool DisableShake = false;
    public float duration = 1f;
    public AnimationCurve curve;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
    }

    public void StartShake(float _duration)
    {
        if (DisableShake)
            return;
        SetStartPosition(transform.position);
        duration = _duration;
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
