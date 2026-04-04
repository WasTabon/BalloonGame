using UnityEngine;
using DG.Tweening;

public class DecorBalloon : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(transform.position.y + 0.5f, 1.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOScale(transform.localScale * 1.03f, 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
