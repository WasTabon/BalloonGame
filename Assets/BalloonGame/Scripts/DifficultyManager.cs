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
    }

    private void Start()
    {
        GameMode mode = GameModeManager.Instance != null ? GameModeManager.Instance.CurrentMode : GameMode.Classic;
        int startScore = 0;
        if (mode == GameMode.TimeAttack) startScore = 50;
        UpdateDifficulty(startScore);
    }

    public void UpdateDifficulty(int score)
    {
        GameMode mode = GameModeManager.Instance != null ? GameModeManager.Instance.CurrentMode : GameMode.Classic;

        int effectiveScore = score;
        if (mode == GameMode.TimeAttack) effectiveScore = Mathf.Max(score + 50, score);

        float rawT = Mathf.Clamp01(effectiveScore / 150f);
        float t = rawT * rawT;

        BalloonSpeed = Mathf.Lerp(baseBalloonSpeed, maxBalloonSpeed, t);
        SpawnInterval = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, rawT);
        ObstacleSpeed = Mathf.Lerp(baseObstacleSpeed, maxObstacleSpeed, t);
        ObstacleMass = Mathf.Lerp(baseObstacleMass, maxObstacleMass, t);

        if (mode == GameMode.ShieldRush)
        {
            SpawnInterval *= 0.35f;
            ObstacleMass *= 0.6f;
        }

        CanSpawnLine = effectiveScore >= 8;
        CanSpawnRain = effectiveScore >= 18;
        CanSpawnSide = effectiveScore >= 30;
        CanSpawnNarrow = effectiveScore >= 50;
        CanSpawnTrap = effectiveScore >= 75;
    }
}
