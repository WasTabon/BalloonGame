using UnityEngine;
using System.Collections.Generic;

public class BackgroundGrid : MonoBehaviour
{
    [SerializeField] private int lineCount = 12;
    [SerializeField] private float lineSpacing = 2f;
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private float lineAlpha = 0.04f;

    private Camera mainCam;
    private List<Transform> lines = new List<Transform>();
    private Material lineMaterial;

    private void Start()
    {
        mainCam = Camera.main;
        lineMaterial = new Material(Shader.Find("Sprites/Default"));

        var tex = new Texture2D(4, 4);
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        var sprite = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4);

        float camHalfW = mainCam.orthographicSize * mainCam.aspect;
        float camY = mainCam.transform.position.y;

        for (int i = 0; i < lineCount; i++)
        {
            var go = new GameObject($"GridLine_{i}");
            go.transform.SetParent(transform);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.material = lineMaterial;
            sr.color = new Color(0.4f, 0.4f, 0.6f, lineAlpha);
            sr.sortingOrder = -15;

            float width = camHalfW * 2.5f;
            go.transform.localScale = new Vector3(width, 0.02f, 1f);

            float y = camY - mainCam.orthographicSize + i * lineSpacing;
            go.transform.position = new Vector3(0, y, 0.5f);
            lines.Add(go.transform);
        }
    }

    private void Update()
    {
        if (mainCam == null) return;

        float camY = mainCam.transform.position.y;
        float camHalfH = mainCam.orthographicSize;
        float totalHeight = lineCount * lineSpacing;

        foreach (var line in lines)
        {
            line.position += Vector3.down * scrollSpeed * Time.deltaTime;

            if (line.position.y < camY - camHalfH - lineSpacing)
            {
                float newY = line.position.y + totalHeight;
                line.position = new Vector3(line.position.x, newY, line.position.z);
            }
        }
    }
}
