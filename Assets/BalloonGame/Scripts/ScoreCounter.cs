using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreCounter : MonoBehaviour
{
    public static void AnimateScore(TextMeshProUGUI text, int targetScore, float duration = 0.8f)
    {
        int current = 0;
        DOTween.To(() => current, x =>
        {
            current = x;
            text.text = current.ToString();
        }, targetScore, duration)
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            text.text = targetScore.ToString();
            text.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 8, 0.5f);
        });
    }
}
