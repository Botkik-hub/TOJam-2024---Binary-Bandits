using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float duration;

    private void Start()
    {
      // Sequence sequence = DOTween.Sequence().SetLoops(-1);
       transform.DOMove(targetPos+transform.position, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
       //sequence.Append(transform.DOMove(targetPos, duration));
       //transform.DO
    }
}
