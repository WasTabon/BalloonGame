using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    private Camera mainCam;
    private EdgeCollider2D edgeCollider;

    private void Start()
    {
        mainCam = Camera.main;
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        UpdateBounds();
    }

    private void LateUpdate()
    {
        UpdateBounds();
    }

    private void UpdateBounds()
    {
        float camHalfH = mainCam.orthographicSize;
        float camHalfW = camHalfH * mainCam.aspect;
        float camY = mainCam.transform.position.y;
        float margin = 0.5f;

        float left = -camHalfW - margin;
        float right = camHalfW + margin;
        float top = camY + camHalfH + 5f;
        float bottom = camY - camHalfH - 5f;

        Vector2[] points = new Vector2[]
        {
            new Vector2(left, bottom),
            new Vector2(left, top),
            new Vector2(right, top),
            new Vector2(right, bottom),
        };

        edgeCollider.points = points;
    }
}
