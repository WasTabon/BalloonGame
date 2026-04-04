using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup dimBg;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private TextMeshProUGUI soundToggleText;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        soundToggleButton.onClick.RemoveListener(OnSoundToggle);
        soundToggleButton.onClick.AddListener(OnSoundToggle);
        closeButton.onClick.RemoveListener(Hide);
        closeButton.onClick.AddListener(Hide);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSoundChanged -= UpdateSoundText;
            GameManager.Instance.OnSoundChanged += UpdateSoundText;
        }
    }

    private void OnDisable()
    {
        soundToggleButton.onClick.RemoveListener(OnSoundToggle);
        closeButton.onClick.RemoveListener(Hide);

        if (GameManager.Instance != null)
            GameManager.Instance.OnSoundChanged -= UpdateSoundText;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateSoundText(GameManager.Instance.SoundEnabled);

        dimBg.alpha = 0f;
        dimBg.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);

        panel.localScale = Vector3.zero;
        panel.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();

        dimBg.DOFade(0f, 0.2f).SetEase(Ease.InQuad);
        panel.DOScale(0f, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnSoundToggle()
    {
        GameManager.Instance.ToggleSound();

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();

        if (MusicManager.Instance != null)
            MusicManager.Instance.UpdateVolume();

        soundToggleButton.transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 8, 0.5f);
    }

    private void UpdateSoundText(bool enabled)
    {
        soundToggleText.text = enabled ? "SOUND: ON" : "SOUND: OFF";
    }
}
