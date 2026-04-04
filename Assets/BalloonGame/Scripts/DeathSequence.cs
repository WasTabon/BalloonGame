using UnityEngine;
using DG.Tweening;

public class DeathSequence : MonoBehaviour
{
    public static DeathSequence Instance { get; private set; }

    [SerializeField] private float slowMoDuration = 0.3f;
    [SerializeField] private float slowMoScale = 0.2f;
    [SerializeField] private float pauseDuration = 0.4f;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(System.Action onComplete)
    {
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, slowMoScale, slowMoDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(pauseDuration, () =>
                {
                    Time.timeScale = 1f;
                    onComplete?.Invoke();
                }, false).SetUpdate(true);
            });
    }
}
