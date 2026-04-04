using UnityEngine;
using System.Collections.Generic;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private int layerCount = 3;
    [SerializeField] private int dotsPerLayer = 30;
    [SerializeField] private float baseSpeed = 0.3f;

    private Camera mainCam;
    private List<List<Transform>> layers = new List<List<Transform>>();
    private List<float> layerSpeeds = new List<float>();
    private Material dotMaterial;

    private void Start()
    {
        mainCam = Camera.main;
        dotMaterial = new Material(Shader.Find("Sprites/Default"));

        for (int l = 0; l < layerCount; l++)
        {
            float depth = (l + 1) * 0.3f;
            float speed = baseSpeed * (1f + l * 0.5f);
            float alpha = 0.08f + l * 0.04f;
            float size = 0.03f + l * 0.02f;
            Color color = new Color(0.5f, 0.5f, 0.7f, alpha);

            layerSpeeds.Add(speed);
            var dots = new List<Transform>();

            for (int i = 0; i < dotsPerLayer; i++)
            {
                var go = new GameObject($"Dot_L{l}_{i}");
                go.transform.SetParent(transform);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.material = dotMaterial;
                sr.color = color;
                sr.sortingOrder = -10 + l;

                var tex = new Texture2D(4, 4);
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        tex.SetPixel(x, y, Color.white);
                tex.Apply();
                sr.sprite = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 128);

                go.transform.localScale = Vector3.one * size;
                go.transform.position = GetRandomPosition(depth);
                dots.Add(go.transform);
            }
            layers.Add(dots);
        }
    }

    private void Update()
    {
        if (mainCam == null) return;

        float camHalfH = mainCam.orthographicSize;
        float camHalfW = camHalfH * mainCam.aspect;
        float camY = mainCam.transform.position.y;

        for (int l = 0; l < layers.Count; l++)
        {
            float speed = layerSpeeds[l];
            float depth = (l + 1) * 0.3f;

            foreach (var dot in layers[l])
            {
                dot.position += Vector3.down * speed * Time.deltaTime;

                if (dot.position.y < camY - camHalfH - 2f)
                {
                    float newX = Random.Range(-camHalfW * 1.2f, camHalfW * 1.2f);
                    float newY = camY + camHalfH + Random.Range(1f, 3f);
                    dot.position = new Vector3(newX, newY, depth);
                }
            }
        }
    }

    private Vector3 GetRandomPosition(float z)
    {
        if (mainCam == null) mainCam = Camera.main;
        float camHalfH = mainCam.orthographicSize;
        float camHalfW = camHalfH * mainCam.aspect;
        float camY = mainCam.transform.position.y;

        float x = Random.Range(-camHalfW * 1.2f, camHalfW * 1.2f);
        float y = Random.Range(camY - camHalfH - 2f, camY + camHalfH + 2f);
        return new Vector3(x, y, z);
    }
}
