using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class TapToStart : MonoBehaviour
{
    public static TapToStart Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tapText;

    private bool started;
    private Tween pulseTween;

    public bool HasStarted => started;
    public System.Action OnGameStarted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tapText.alpha = 0f;
        tapText.DOFade(1f, 0.5f).SetDelay(0.5f).OnComplete(() =>
        {
            pulseTween = tapText.DOFade(0.3f, 0.8f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void Update()
    {
        if (started) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            if (Input.touchCount > 0 && EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            StartGame();
        }
    }

    private void StartGame()
    {
        started = true;
        pulseTween?.Kill();

        tapText.DOFade(0f, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            tapText.gameObject.SetActive(false);
        });

        OnGameStarted?.Invoke();

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayButtonClick();
    }
}
