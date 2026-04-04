using UnityEngine;
using DG.Tweening;

public class ObstacleHitFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    private bool flashing;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (flashing) return;
        if (collision.gameObject.GetComponent<Shield>() == null) return;

        Flash();
    }

    private void Flash()
    {
        if (sr == null) return;
        flashing = true;

        sr.DOKill();
        sr.DOColor(Color.white, 0.05f).OnComplete(() =>
        {
            sr.DOColor(originalColor, 0.15f).OnComplete(() =>
            {
                flashing = false;
            });
        });
    }
}
