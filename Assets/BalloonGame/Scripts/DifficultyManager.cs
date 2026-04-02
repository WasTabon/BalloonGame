using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [SerializeField] private float baseBalloonSpeed = 3f;
    [SerializeField] private float maxBalloonSpeed = 6f;
    [SerializeField] private float baseSpawnInterval = 1.2f;
    [SerializeField] private float minSpawnInterval = 0.4f;
    [SerializeField] private float baseObstacleSpeed = 1f;
    [SerializeField] private float maxObstacleSpeed = 4f;
    [SerializeField] private float baseObstacleMass = 0.5f;
    [SerializeField] private float maxObstacleMass = 4f;

    public float BalloonSpeed { get; private set; }
    public float SpawnInterval { get; private set; }
    public float ObstacleSpeed { get; private set; }
    public float ObstacleMass { get; private set; }

    public bool CanSpawnLine { get; private set; }
    public bool CanSpawnRain { get; private set; }
    public bool CanSpawnSide { get; private set; }
    public bool CanSpawnNarrow { get; private set; }
    public bool CanSpawnTrap { get; private set; }

    private void Awake()
    {
        Instance = this;
        UpdateDifficulty(0);
    }

    public void UpdateDifficulty(int score)
    {
        float t = Mathf.Clamp01(score / 100f);

        BalloonSpeed = Mathf.Lerp(baseBalloonSpeed, maxBalloonSpeed, t);
        SpawnInterval = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, t);
        ObstacleSpeed = Mathf.Lerp(baseObstacleSpeed, maxObstacleSpeed, t);
        ObstacleMass = Mathf.Lerp(baseObstacleMass, maxObstacleMass, t);

        CanSpawnLine = score >= 10;
        CanSpawnRain = score >= 20;
        CanSpawnSide = score >= 30;
        CanSpawnNarrow = score >= 50;
        CanSpawnTrap = score >= 70;
    }
}
