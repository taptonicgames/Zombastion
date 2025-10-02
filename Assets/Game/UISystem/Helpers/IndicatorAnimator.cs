using DG.Tweening;
using UnityEngine;

public class IndicatorAnimator : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private float duration;
    [SerializeField] private float delay;

    private Sequence sequence;

    private void OnEnable()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.Append(transform.DOScale(targetScale, duration).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo));
        sequence.AppendInterval(delay);
    }

    private void OnDisable()
    {
        sequence.Kill();
    }
}
