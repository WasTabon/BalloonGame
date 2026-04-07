using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI modeLabelText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PausePopup pausePopup;

    private int displayedScore;
    private int lastMilestone;
    private float timeRemaining;
    private bool timerActive;
    private bool timerWarning;

    private void Start()
    {
        scoreText.text = "0";
        lastMilestone = 0;
        pauseButton.onClick.AddListener(OnPauseClicked);

        scoreText.outlineWidth = 0.2f;
        scoreText.outlineColor = new Color32(0, 0, 0, 128);

        GameplayManager.Instance.OnScoreChanged -= UpdateScore;
        GameplayManager.Instance.OnScoreChanged += UpdateScore;

        GameMode mode = GameModeManager.Instance != null ? GameModeManager.Instance.CurrentMode : GameMode.Classic;

        if (modeLabelText != null)
        {
            modeLabelText.text = GameModeManager.Instance != null ? GameModeManager.Instance.GetModeName() : "CLASSIC";
            modeLabelText.alpha = 0f;
            modeLabelText.DOFade(0.5f, 0.5f).SetDelay(0.2f);
        }

        if (mode == GameMode.TimeAttack)
        {
            timeRemaining = 30f;
            timerText.gameObject.SetActive(true);
            timerText.text = "30";
            timerText.outlineWidth = 0.2f;
            timerText.outlineColor = new Color32(0, 0, 0, 128);

            if (TapToStart.Instance != null)
            {
                TapToStart.Instance.OnGameStarted -= StartTimer;
                TapToStart.Instance.OnGameStarted += StartTimer;
            }
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }

        scoreText.transform.localScale = Vector3.zero;
        scoreText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetDelay(0.3f);

        pauseButton.transform.localScale = Vector3.zero;
        pauseButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }

    private void OnDestroy()
    {
        if (GameplayManager.Instance != null)
            GameplayManager.Instance.OnScoreChanged -= UpdateScore;

        if (TapToStart.Instance != null)
            TapToStart.Instance.OnGameStarted -= StartTimer;

        pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    private void StartTimer()
    {
        timerActive = true;
    }

    private void Update()
    {
        if (!timerActive) return;
        if (GameplayManager.Instance.IsGameOver || GameplayManager.Instance.IsPaused) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerActive = false;
            timerText.text = "0";
            GameplayManager.Instance.TriggerGameOver();
            return;
        }

        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 10f && !timerWarning)
        {
            timerWarning = true;
            timerText.color = new Color(1f, 0.4f, 0.4f);
            timerText.DOKill();
            timerText.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void UpdateScore(int score)
    {
        displayedScore = score;
        scoreText.text = score.ToString();

        int currentMilestone = (score / 10) * 10;
        if (currentMilestone > lastMilestone && currentMilestone > 0)
        {
            lastMilestone = currentMilestone;
            scoreText.transform.DOKill(true);
            scoreText.transform.DOPunchScale(Vector3.one * 0.35f, 0.4f, 8, 0.5f);

            Color origColor = scoreText.color;
            scoreText.DOColor(new Color(1f, 0.9f, 0.3f), 0.1f).OnComplete(() =>
            {
                scoreText.DOColor(origColor, 0.3f);
            });

            if (ScreenShake.Instance != null)
                ScreenShake.Instance.ShakeMedium();

            if (ParticleManager.Instance != null)
            {
                Vector3 worldPos = Camera.main.transform.position + new Vector3(0, Camera.main.orthographicSize * 0.8f, 10f);
                ParticleManager.Instance.PlayScoreMilestone(worldPos);
            }

            if (SFXManager.Instance != null)
                SFXManager.Instance.PlayMilestone();

            if (HapticManager.Instance != null)
                HapticManager.Instance.Medium();
        }
        else
        {
            scoreText.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f, 6, 0.5f);

            if (SFXManager.Instance != null)
                SFXManager.Instance.PlayScoreTick();
        }
    }

    private void OnPauseClicked()
    {
        if (GameplayManager.Instance.IsGameOver) return;
        GameplayManager.Instance.SetPaused(true);
        pausePopup.Show();
    }
}
