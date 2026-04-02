using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup dimBg;
    [SerializeField] private RectTransform panel;
    [SerializeField] private TextMeshProUGUI scoreValueText;
    [SerializeField] private TextMeshProUGUI bestValueText;
    [SerializeField] private TextMeshProUGUI newBestText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        restartButton.onClick.RemoveListener(OnRestartClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        menuButton.onClick.RemoveListener(OnMenuClicked);
        menuButton.onClick.AddListener(OnMenuClicked);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(OnRestartClicked);
        menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    public void Show(int score, int bestScore, bool isNewBest)
    {
        gameObject.SetActive(true);

        scoreValueText.text = score.ToString();
        bestValueText.text = bestScore.ToString();
        newBestText.gameObject.SetActive(isNewBest);

        dimBg.alpha = 0f;
        dimBg.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);

        panel.localScale = Vector3.zero;
        panel.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetDelay(0.1f);

        if (isNewBest)
        {
            newBestText.transform.localScale = Vector3.zero;
            newBestText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic).SetDelay(0.5f);
        }

        restartButton.transform.localScale = Vector3.zero;
        restartButton.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack).SetDelay(0.4f);
        menuButton.transform.localScale = Vector3.zero;
        menuButton.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }

    private void OnRestartClicked()
    {
        SceneLoader.Instance.LoadScene("Game");
    }

    private void OnMenuClicked()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
