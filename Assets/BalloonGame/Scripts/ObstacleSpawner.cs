using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Sprite squareSprite;
    [SerializeField] private Sprite rectSprite;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private float spawnOffsetY = 2f;

    private Camera mainCam;
    private float timer;
    private List<GameObject> activeObstacles = new List<GameObject>();

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (GameplayManager.Instance.IsGameOver || GameplayManager.Instance.IsPaused) return;
        if (TapToStart.Instance != null && !TapToStart.Instance.HasStarted) return;

        timer += Time.deltaTime;
        float interval = DifficultyManager.Instance != null ? DifficultyManager.Instance.SpawnInterval : 1.2f;

        if (timer >= interval)
        {
            timer = 0f;
            SpawnPattern();
        }

        activeObstacles.RemoveAll(o => o == null);
    }

    private void SpawnPattern()
    {
        var dm = DifficultyManager.Instance;
        List<int> available = new List<int> { 0 };
        if (dm != null)
        {
            if (dm.CanSpawnLine) available.Add(1);
            if (dm.CanSpawnRain) available.Add(2);
            if (dm.CanSpawnSide) available.Add(3);
            if (dm.CanSpawnNarrow) available.Add(4);
            if (dm.CanSpawnTrap) available.Add(5);
        }

        int pattern = available[Random.Range(0, available.Count)];
        switch (pattern)
        {
            case 0: SpawnSingle(); break;
            case 1: SpawnLine(); break;
            case 2: SpawnRain(); break;
            case 3: SpawnSide(); break;
            case 4: SpawnNarrow(); break;
            case 5: SpawnTrap(); break;
        }
    }

    private void SpawnSingle()
    {
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;
        float camHalfW = mainCam.orthographicSize * mainCam.aspect;
        float x = Random.Range(-camHalfW * 0.85f, camHalfW * 0.85f);
        float y = camTop + spawnOffsetY;
        SpawnRandomObstacle(new Vector3(x, y, 0), Vector2.zero);
    }

    private void SpawnLine()
    {
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;
        float camHalfW = mainCam.orthographicSize * mainCam.aspect;
        float y = camTop + spawnOffsetY;

        int count = Random.Range(3, 6);
        int gapIndex = Random.Range(0, count);
        float totalWidth = camHalfW * 2f * 0.9f;
        float blockWidth = totalWidth / count;

        for (int i = 0; i < count; i++)
        {
            if (i == gapIndex) continue;
            float x = -camHalfW * 0.9f + blockWidth * (i + 0.5f);
            var obs = CreateObstacle(new Vector3(x, y, 0));
            var sr = obs.GetComponent<SpriteRenderer>();
            sr.sprite = squareSprite;
            float size = blockWidth * 0.85f;
            obs.transform.localScale = new Vector3(size, size * Random.Range(0.6f, 1f), 1f);
            obs.AddComponent<BoxCollider2D>();
            SetupRigidbody(obs, new Vector2(0, -0.5f));
        }
    }

    private void SpawnRain()
    {
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;
        float camHalfW = mainCam.orthographicSize * mainCam.aspect;

        int count = Random.Range(5, 9);
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-camHalfW * 0.9f, camHalfW * 0.9f);
            float y = camTop + spawnOffsetY + Random.Range(0f, 3f);
            var obs = CreateObstacle(new Vector3(x, y, 0));
            var sr = obs.GetComponent<SpriteRenderer>();
            sr.sprite = circleSprite;
            float size = Random.Range(0.2f, 0.45f);
            obs.transform.localScale = new Vector3(size, size, 1f);
            obs.AddComponent<CircleCollider2D>();
            float speed = DifficultyManager.Instance != null ? DifficultyManager.Instance.ObstacleSpeed : 1f;
            SetupRigidbody(obs, new Vector2(Random.Range(-0.5f, 0.5f), -speed * Random.Range(0.5f, 1.5f)));
        }
    }

    private void SpawnSide()
    {
        float camHalfW = mainCam.orthographicSize * mainCam.aspect;
        float camY = mainCam.transform.position.y;

        bool fromLeft = Random.value > 0.5f;
        float x = fromLeft ? -camHalfW - 2f : camHalfW + 2f;
        float y = camY + Random.Range(-mainCam.orthographicSize * 0.3f, mainCam.orthographicSize * 0.8f);
        float speed = DifficultyManager.Instance != null ? DifficultyManager.Instance.ObstacleSpeed : 2f;
        float vx = fromLeft ? speed * Random.Range(1.5f, 3f) : -speed * Random.Range(1.5f, 3f);

        int count = Random.Range(1, 4);
        for (int i = 0; i < count; i++)
        {
            float offsetY = i * Random.Range(0.8f, 1.5f);
            var obs = CreateObstacle(new Vector3(x, y + offsetY, 0));
            var sr = obs.GetComponent<SpriteRenderer>();
            int shape = Random.Range(0, 2);
            if (shape == 0)
            {
                sr.sprite = squareSprite;
                float size = Random.Range(0.4f, 0.8f);
                obs.transform.localScale = new Vector3(size, size, 1f);
                obs.AddComponent<BoxCollider2D>();
            }
            else
            {
                sr.sprite = circleSprite;
                float size = Random.Range(0.3f, 0.7f);
                obs.transform.localScale = new Vector3(size, size, 1f);
                obs.AddComponent<CircleCollider2D>();
            }
            SetupRigidbody(obs, new Vector2(vx, Random.Range(-0.5f, 0.5f)), 0.2f);
        }
    }

    private void SpawnNarrow()
    {
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;
        float camHalfW = mainCam.orthographicSize * mainCam.aspect;
        float y = camTop + spawnOffsetY;

        float gapCenter = Random.Range(-camHalfW * 0.4f, camHalfW * 0.4f);
        float gapWidth = Random.Range(1.5f, 2.5f);

        float leftEnd = gapCenter - gapWidth * 0.5f;
        float rightStart = gapCenter + gapWidth * 0.5f;

        float leftWidth = leftEnd - (-camHalfW);
        if (leftWidth > 0.5f)
        {
            float leftCenter = -camHalfW + leftWidth * 0.5f;
            var leftObs = CreateObstacle(new Vector3(leftCenter, y, 0));
            leftObs.GetComponent<SpriteRenderer>().sprite = rectSprite;
            leftObs.transform.localScale = new Vector3(leftWidth, Random.Range(0.3f, 0.6f), 1f);
            leftObs.AddComponent<BoxCollider2D>();
            SetupRigidbody(leftObs, new Vector2(0, -0.3f), 0.1f);
        }

        float rightWidth = camHalfW - rightStart;
        if (rightWidth > 0.5f)
        {
            float rightCenter = rightStart + rightWidth * 0.5f;
            var rightObs = CreateObstacle(new Vector3(rightCenter, y, 0));
            rightObs.GetComponent<SpriteRenderer>().sprite = rectSprite;
            rightObs.transform.localScale = new Vector3(rightWidth, Random.Range(0.3f, 0.6f), 1f);
            rightObs.AddComponent<BoxCollider2D>();
            SetupRigidbody(rightObs, new Vector2(0, -0.3f), 0.1f);
        }
    }

    private void SpawnTrap()
    {
        SpawnLine();
        SpawnSide();
    }

    private void SpawnRandomObstacle(Vector3 position, Vector2 extraVelocity)
    {
        var obs = CreateObstacle(position);
        var sr = obs.GetComponent<SpriteRenderer>();

        int type = Random.Range(0, 3);
        switch (type)
        {
            case 0:
                sr.sprite = squareSprite;
                float sqSize = Random.Range(0.4f, 1.0f);
                obs.transform.localScale = new Vector3(sqSize, sqSize, 1f);
                obs.AddComponent<BoxCollider2D>();
                break;
            case 1:
                sr.sprite = rectSprite;
                float rw = Random.Range(0.8f, 2.0f);
                float rh = Random.Range(0.2f, 0.5f);
                obs.transform.localScale = new Vector3(rw, rh, 1f);
                obs.AddComponent<BoxCollider2D>();
                break;
            default:
                sr.sprite = circleSprite;
                float cSize = Random.Range(0.3f, 0.8f);
                obs.transform.localScale = new Vector3(cSize, cSize, 1f);
                obs.AddComponent<CircleCollider2D>();
                break;
        }

        float speed = DifficultyManager.Instance != null ? DifficultyManager.Instance.ObstacleSpeed : 1f;
        Vector2 vel = new Vector2(Random.Range(-speed, speed), -Random.Range(0, speed)) + extraVelocity;
        SetupRigidbody(obs, vel);
    }

    private GameObject CreateObstacle(Vector3 position)
    {
        GameObject obs = new GameObject("Obstacle");
        obs.transform.position = position;

        var sr = obs.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 3;
        sr.color = GetRandomColor();

        obs.AddComponent<Obstacle>();
        obs.AddComponent<ObstacleHitFlash>();
        activeObstacles.Add(obs);
        return obs;
    }

    private void SetupRigidbody(GameObject obs, Vector2 velocity, float gravityScale = -1f)
    {
        var rb = obs.GetComponent<Rigidbody2D>();
        if (rb == null) rb = obs.AddComponent<Rigidbody2D>();

        float mass = DifficultyManager.Instance != null ? DifficultyManager.Instance.ObstacleMass : 1f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = Random.Range(mass * 0.5f, mass * 1.5f);
        rb.gravityScale = gravityScale < 0 ? Random.Range(0.3f, 1.2f) : gravityScale;
        rb.drag = 0.2f;
        rb.angularDrag = 0.3f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.velocity = velocity;

        float angularVel = Random.Range(0f, 180f);
        if (Random.value > 0.5f) angularVel = -angularVel;
        rb.angularVelocity = angularVel;
    }

    private Color GetRandomColor()
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
            if (obs != null) Destroy(obs);
        }
        activeObstacles.Clear();
    }
}
