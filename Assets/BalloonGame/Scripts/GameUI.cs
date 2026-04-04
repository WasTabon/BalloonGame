using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PausePopup pausePopup;

    private int displayedScore;
    private int lastMilestone;

    private void Start()
    {
        scoreText.text = "0";
        lastMilestone = 0;
        pauseButton.onClick.AddListener(OnPauseClicked);

        GameplayManager.Instance.OnScoreChanged -= UpdateScore;
        GameplayManager.Instance.OnScoreChanged += UpdateScore;

        scoreText.transform.localScale = Vector3.zero;
        scoreText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetDelay(0.3f);

        pauseButton.transform.localScale = Vector3.zero;
        pauseButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }

    private void OnDestroy()
    {
        if (GameplayManager.Instance != null)
            GameplayManager.Instance.OnScoreChanged -= UpdateScore;

        pauseButton.onClick.RemoveListener(OnPauseClicked);
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
