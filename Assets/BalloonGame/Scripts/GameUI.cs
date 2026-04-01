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

    private void Start()
    {
        scoreText.text = "0";
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

        scoreText.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f, 6, 0.5f);
    }

    private void OnPauseClicked()
    {
        if (GameplayManager.Instance.IsGameOver) return;
        GameplayManager.Instance.SetPaused(true);
        pausePopup.Show();
    }
}
