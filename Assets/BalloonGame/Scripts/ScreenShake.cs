using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private Vector3 originalOffset;
    private Tween shakeTween;

    private void Awake()
    {
        Instance = this;
    }

    public void ShakeLight()
    {
        Shake(0.15f, 0.15f, 20);
    }

    public void ShakeMedium()
    {
        Shake(0.25f, 0.3f, 25);
    }

    public void ShakeHeavy()
    {
        Shake(0.4f, 0.5f, 30);
    }

    private void Shake(float duration, float strength, int vibrato)
    {
        shakeTween?.Kill(true);
        shakeTween = transform.DOShakePosition(duration, strength, vibrato, 90f, false, true, ShakeRandomnessMode.Harmonic)
            .SetUpdate(true);
    }
}
