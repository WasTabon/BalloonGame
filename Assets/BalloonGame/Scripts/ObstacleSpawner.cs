using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float minSpawnInterval = 0.3f;
    [SerializeField] private float intervalDecreaseRate = 0.01f;
    [SerializeField] private float spawnOffsetY = 2f;
    [SerializeField] private float obstacleMinMass = 0.5f;
    [SerializeField] private float obstacleMaxMass = 3f;
    [SerializeField] private float obstacleMinSpeed = 0f;
    [SerializeField] private float obstacleMaxSpeed = 2f;
    [SerializeField] private float obstacleMinAngularSpeed = 0f;
    [SerializeField] private float obstacleMaxAngularSpeed = 180f;

    [SerializeField] private Sprite squareSprite;
    [SerializeField] private Sprite rectSprite;
    [SerializeField] private Sprite circleSprite;

    private Camera mainCam;
    private float timer;
    private float currentInterval;
    private List<GameObject> activeObstacles = new List<GameObject>();

    private void Start()
    {
        mainCam = Camera.main;
        currentInterval = spawnInterval;
    }

    private void Update()
    {
        if (GameplayManager.Instance.IsGameOver || GameplayManager.Instance.IsPaused) return;

        currentInterval = Mathf.Max(minSpawnInterval, currentInterval - intervalDecreaseRate * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        activeObstacles.RemoveAll(o => o == null);
    }

    private void SpawnObstacle()
    {
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;
        float camHalfWidth = mainCam.orthographicSize * mainCam.aspect;
        float spawnX = Random.Range(-camHalfWidth * 0.9f, camHalfWidth * 0.9f);
        float spawnY = camTop + spawnOffsetY;

        int type = Random.Range(0, 3);
        GameObject obs = new GameObject("Obstacle");
        obs.transform.position = new Vector3(spawnX, spawnY, 0);

        var sr = obs.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 3;
        sr.color = GetRandomObstacleColor();

        float mass;
        switch (type)
        {
            case 0:
                sr.sprite = squareSprite;
                float sqSize = Random.Range(0.4f, 1.0f);
                obs.transform.localScale = new Vector3(sqSize, sqSize, 1f);
                var boxCol = obs.AddComponent<BoxCollider2D>();
                mass = Random.Range(obstacleMinMass, obstacleMaxMass);
                break;
            case 1:
                sr.sprite = rectSprite;
                float rw = Random.Range(0.8f, 2.0f);
                float rh = Random.Range(0.2f, 0.5f);
                obs.transform.localScale = new Vector3(rw, rh, 1f);
                var rectCol = obs.AddComponent<BoxCollider2D>();
                mass = Random.Range(obstacleMinMass * 1.5f, obstacleMaxMass * 1.5f);
                break;
            default:
                sr.sprite = circleSprite;
                float cSize = Random.Range(0.3f, 0.8f);
                obs.transform.localScale = new Vector3(cSize, cSize, 1f);
                var circCol = obs.AddComponent<CircleCollider2D>();
                mass = Random.Range(obstacleMinMass, obstacleMaxMass);
                break;
        }

        var rb = obs.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = mass;
        rb.gravityScale = Random.Range(0.3f, 1.2f);
        rb.drag = 0.2f;
        rb.angularDrag = 0.3f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        float sideSpeed = Random.Range(-obstacleMaxSpeed, obstacleMaxSpeed);
        float downSpeed = Random.Range(obstacleMinSpeed, obstacleMaxSpeed);
        rb.velocity = new Vector2(sideSpeed, -downSpeed);

        float angularVel = Random.Range(obstacleMinAngularSpeed, obstacleMaxAngularSpeed);
        if (Random.value > 0.5f) angularVel = -angularVel;
        rb.angularVelocity = angularVel;

        obs.AddComponent<Obstacle>();

        activeObstacles.Add(obs);
    }

    private Color GetRandomObstacleColor()
    {
        Color[] colors = new Color[]
        {
            new Color(0.85f, 0.35f, 0.35f),
            new Color(0.9f, 0.6f, 0.2f),
            new Color(0.85f, 0.85f, 0.3f),
            new Color(0.4f, 0.8f, 0.4f),
            new Color(0.4f, 0.5f, 0.9f),
            new Color(0.7f, 0.4f, 0.85f),
        };
        return colors[Random.Range(0, colors.Length)];
    }

    public void ClearAll()
    {
        foreach (var obs in activeObstacles)
        {
            if (obs != null)
                Destroy(obs);
        }
        activeObstacles.Clear();
    }
}
