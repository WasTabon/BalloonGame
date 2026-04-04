using UnityEngine;

public class BalloonTrail : MonoBehaviour
{
    private TrailRenderer trail;

    private void Awake()
    {
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = 0.3f;
        trail.startWidth = transform.localScale.x * 0.3f;
        trail.endWidth = 0f;
        trail.startColor = new Color(1f, 0.45f, 0.5f, 0.3f);
        trail.endColor = new Color(1f, 0.45f, 0.5f, 0f);
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.sortingOrder = 4;
        trail.minVertexDistance = 0.05f;
        trail.numCornerVertices = 3;
    }
}
