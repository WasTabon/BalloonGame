using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [SerializeField] private Balloon balloon;
    [SerializeField] private Shield shield;
    [SerializeField] private GameOverPopup gameOverPopup;

    private float startY;
    private bool isGameOver;
    private bool isPaused;

    public int CurrentScore { get; private set; }
    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;

    public System.Action<int> OnScoreChanged;
    public System.Action OnGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Assert(balloon != null, "GameplayManager: balloon is null!");
        Debug.Assert(shield != null, "GameplayManager: shield is null!");
        Debug.Assert(gameOverPopup != null, "GameplayManager: gameOverPopup is null!");

        startY = balloon.transform.position.y;

        balloon.OnBalloonHit -= TriggerGameOver;
        balloon.OnBalloonHit += TriggerGameOver;
    }

    private void OnDestroy()
    {
        if (balloon != null)
            balloon.OnBalloonHit -= TriggerGameOver;
    }

    private void Update()
    {
        if (isGameOver || isPaused) return;

        int newScore = Mathf.Max(0, Mathf.FloorToInt(balloon.GetHeight() - startY));
        if (newScore > CurrentScore)
        {
            CurrentScore = newScore;
            OnScoreChanged?.Invoke(CurrentScore);

            if (DifficultyManager.Instance != null)
                DifficultyManager.Instance.UpdateDifficulty(CurrentScore);
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        bool isNewBest = GameManager.Instance.TrySetBestScore(CurrentScore);
        OnGameOver?.Invoke();
        gameOverPopup.Show(CurrentScore, GameManager.Instance.BestScore, isNewBest);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }
}
