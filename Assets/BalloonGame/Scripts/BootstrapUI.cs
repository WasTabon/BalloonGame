using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BootstrapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image progressFill;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button retryButton;

    private void Start()
    {
        retryButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(OnRetryClicked);
        progressFill.fillAmount = 0f;

        titleText.alpha = 0f;
        statusText.alpha = 0f;
        titleText.DOFade(1f, 0.8f).SetEase(Ease.OutQuad);
        statusText.DOFade(1f, 0.5f).SetDelay(0.3f).SetEase(Ease.OutQuad);

        Debug.Assert(AddressableLoader.Instance != null, "AddressableLoader.Instance is null! Make sure AddressableLoader is on the scene.");

        AddressableLoader.Instance.OnDownloadProgress -= UpdateProgress;
        AddressableLoader.Instance.OnDownloadProgress += UpdateProgress;
        AddressableLoader.Instance.OnStatusChanged -= UpdateStatus;
        AddressableLoader.Instance.OnStatusChanged += UpdateStatus;
        AddressableLoader.Instance.OnDownloadComplete -= OnComplete;
        AddressableLoader.Instance.OnDownloadComplete += OnComplete;
        AddressableLoader.Instance.OnDownloadFailed -= OnFailed;
        AddressableLoader.Instance.OnDownloadFailed += OnFailed;

        AddressableLoader.Instance.StartLoading();
    }

    private void OnDestroy()
    {
        if (AddressableLoader.Instance != null)
        {
            AddressableLoader.Instance.OnDownloadProgress -= UpdateProgress;
            AddressableLoader.Instance.OnStatusChanged -= UpdateStatus;
            AddressableLoader.Instance.OnDownloadComplete -= OnComplete;
            AddressableLoader.Instance.OnDownloadFailed -= OnFailed;
        }
        retryButton.onClick.RemoveListener(OnRetryClicked);
    }

    private void UpdateProgress(float progress)
    {
        progressFill.DOFillAmount(progress, 0.3f).SetEase(Ease.OutCubic);
    }

    private void UpdateStatus(string status)
    {
        statusText.DOFade(0f, 0.15f).OnComplete(() =>
        {
            statusText.text = status;
            statusText.DOFade(1f, 0.15f);
        });
    }

    private void OnComplete()
    {
        Debug.Assert(SceneLoader.Instance != null, "SceneLoader.Instance is null! Make sure SceneLoader is on the scene.");

        if (MusicManager.Instance != null)
            MusicManager.Instance.LoadAndPlay();

        SceneLoader.Instance.LoadScene("MainMenu");
    }

    private void OnFailed(string error)
    {
        statusText.DOKill();
        statusText.alpha = 1f;
        statusText.text = error;
        statusText.color = new Color(1f, 0.4f, 0.4f);

        retryButton.gameObject.SetActive(true);
        retryButton.transform.localScale = Vector3.zero;
        retryButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    private void OnRetryClicked()
    {
        statusText.color = Color.white;
        retryButton.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            retryButton.gameObject.SetActive(false);
        });
        progressFill.fillAmount = 0f;
        AddressableLoader.Instance.StartLoading();
    }
}
