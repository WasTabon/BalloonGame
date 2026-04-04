using UnityEngine;
using DG.Tweening;

public class BalloonBounce : MonoBehaviour
{
    private Tween breathTween;
    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
        StartBreathing();
    }

    private void Update()
    {
        if (TapToStart.Instance != null && TapToStart.Instance.HasStarted && breathTween != null)
        {
            breathTween.Kill();
            breathTween = null;
            transform.DOScale(baseScale, 0.3f).SetEase(Ease.OutQuad);
        }
    }

    private void StartBreathing()
    {
        breathTween = transform.DOScale(baseScale * 1.08f, 0.6f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
