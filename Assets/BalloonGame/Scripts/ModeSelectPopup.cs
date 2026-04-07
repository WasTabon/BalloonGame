using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ModeSelectPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup dimBg;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button classicButton;
    [SerializeField] private Button timeAttackButton;
    [SerializeField] private Button shieldRushButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI classicBestText;
    [SerializeField] private TextMeshProUGUI timeAttackBestText;
    [SerializeField] private TextMeshProUGUI shieldRushBestText;

    private void OnEnable()
    {
        classicButton.onClick.RemoveListener(OnClassicClicked);
        classicButton.onClick.AddListener(OnClassicClicked);
        timeAttackButton.onClick.RemoveListener(OnTimeAttackClicked);
        timeAttackButton.onClick.AddListener(OnTimeAttackClicked);
        shieldRushButton.onClick.RemoveListener(OnShieldRushClicked);
        shieldRushButton.onClick.AddListener(OnShieldRushClicked);
        closeButton.onClick.RemoveListener(Hide);
        closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        classicButton.onClick.RemoveListener(OnClassicClicked);
        timeAttackButton.onClick.RemoveListener(OnTimeAttackClicked);
        shieldRushButton.onClick.RemoveListener(OnShieldRushClicked);
        closeButton.onClick.RemoveListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        UpdateBestScores();

        dimBg.alpha = 0f;
        dimBg.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);

        panel.localScale = Vector3.zero;
        panel.DOScale(1f, 0.35f).SetEase(Ease.OutBack);

        classicButton.transform.localScale = Vector3.zero;
        classicButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.15f);
        timeAttackButton.transform.localScale = Vector3.zero;
        timeAttackButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.25f);
        shieldRushButton.transform.localScale = Vector3.zero;
        shieldRushButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.35f);
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

    private void UpdateBestScores()
    {
        classicBestText.text = $"BEST: {GameManager.Instance.GetBestScore(GameMode.Classic)}";
        timeAttackBestText.text = $"BEST: {GameManager.Instance.GetBestScore(GameMode.TimeAttack)}";
        shieldRushBestText.text = $"BEST: {GameManager.Instance.GetBestScore(GameMode.ShieldRush)}";
    }

    private void StartMode(GameMode mode)
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();

        GameModeManager.Instance.SetMode(mode);
        SceneLoader.Instance.LoadScene("Game");
    }

    private void OnClassicClicked() => StartMode(GameMode.Classic);
    private void OnTimeAttackClicked() => StartMode(GameMode.TimeAttack);
    private void OnShieldRushClicked() => StartMode(GameMode.ShieldRush);
}
