using UnityEngine;
using DG.Tweening;

public class ShieldVisuals : MonoBehaviour
{
    private TrailRenderer trail;
    private SpriteRenderer sr;
    private Vector3 baseScale;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        SetupTrail();
    }

    private void SetupTrail()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
        if (trail == null) trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = 0.15f;
        trail.startWidth = transform.localScale.x * 0.4f;
        trail.endWidth = 0f;
        trail.startColor = new Color(0.4f, 0.7f, 1f, 0.4f);
        trail.endColor = new Color(0.4f, 0.7f, 1f, 0f);
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.sortingOrder = 9;
        trail.minVertexDistance = 0.05f;
        trail.numCornerVertices = 3;
    }

    public void PlayHitEffect()
    {
        transform.DOKill(true);
        transform.localScale = baseScale;
        transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 0.5f);

        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.DOColor(Color.white, 0.05f).OnComplete(() =>
            {
                sr.DOColor(originalColor, 0.15f);
            });
        }
    }
}
