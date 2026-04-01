using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [SerializeField] private Balloon balloon;
    [SerializeField] private Shield shield;

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
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        GameManager.Instance.TrySetBestScore(CurrentScore);
        OnGameOver?.Invoke();
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }
}
