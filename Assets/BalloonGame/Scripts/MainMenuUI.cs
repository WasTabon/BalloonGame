using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private SettingsPopup settingsPopup;

    private Tween playPulseTween;

    private void Start()
    {
        UpdateBestScore(GameManager.Instance.BestScore);

        GameManager.Instance.OnBestScoreChanged -= UpdateBestScore;
        GameManager.Instance.OnBestScoreChanged += UpdateBestScore;

        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);

        AnimateEntrance();
    }

    private void OnDestroy()
    {
        playPulseTween?.Kill();

        if (GameManager.Instance != null)
            GameManager.Instance.OnBestScoreChanged -= UpdateBestScore;

        playButton.onClick.RemoveListener(OnPlayClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    private void AnimateEntrance()
    {
        titleText.alpha = 0f;
        titleText.rectTransform.anchoredPosition += new Vector2(0, 80f);
        bestScoreText.alpha = 0f;
        playButton.transform.localScale = Vector3.zero;
        settingsButton.transform.localScale = Vector3.zero;

        var seq = DOTween.Sequence();

        seq.Append(titleText.DOFade(1f, 0.6f).SetEase(Ease.OutQuad));
        seq.Join(titleText.rectTransform.DOAnchorPosY(
            titleText.rectTransform.anchoredPosition.y - 80f, 0.6f).SetEase(Ease.OutBack));

        seq.Append(bestScoreText.DOFade(1f, 0.4f).SetEase(Ease.OutQuad));

        seq.Append(playButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

        seq.Join(settingsButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            playPulseTween = playButton.transform
                .DOScale(1.05f, 0.8f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void UpdateBestScore(int score)
    {
        bestScoreText.text = $"BEST: {score}";
    }

    private void OnPlayClicked()
    {
        playPulseTween?.Kill();
        playButton.interactable = false;

        playButton.transform.DOScale(0.9f, 0.1f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            playButton.transform.DOScale(1.1f, 0.15f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                SceneLoader.Instance.LoadScene("Game");
            });
        });
    }

    private void OnSettingsClicked()
    {
        settingsPopup.Show();
    }
}
