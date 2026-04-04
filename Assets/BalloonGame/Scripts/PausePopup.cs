using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup dimBg;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        resumeButton.onClick.AddListener(OnResumeClicked);
        menuButton.onClick.RemoveListener(OnMenuClicked);
        menuButton.onClick.AddListener(OnMenuClicked);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (MusicManager.Instance != null)
            MusicManager.Instance.SetPaused(true);

        dimBg.alpha = 0f;
        dimBg.DOFade(1f, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);

        panel.localScale = Vector3.zero;
        panel.DOScale(1f, 0.35f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void Hide(System.Action onComplete = null)
    {
        dimBg.DOFade(0f, 0.2f).SetEase(Ease.InQuad).SetUpdate(true);
        panel.DOScale(0f, 0.25f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void OnResumeClicked()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();

        Hide(() =>
        {
            GameplayManager.Instance.SetPaused(false);
            if (MusicManager.Instance != null)
                MusicManager.Instance.SetPaused(false);
        });
    }

    private void OnMenuClicked()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();

        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetPaused(false);

        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
