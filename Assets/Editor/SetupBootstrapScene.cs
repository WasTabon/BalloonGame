using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SetupBootstrapScene
{
    [MenuItem("BalloonGame/(Iteration 1) Setup Bootstrap Scene")]
    public static void Setup()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGo.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        var cam = Camera.main;
        if (cam == null)
        {
            var camGo = new GameObject("Main Camera");
            cam = camGo.AddComponent<Camera>();
            camGo.tag = "MainCamera";
        }
        cam.backgroundColor = new Color(0.06f, 0.06f, 0.1f);
        cam.orthographic = true;

        SetupAddressableLoader();
        SetupSceneLoader();
        SetupMusicManager();
        SetupSFXManager();
        SetupHapticManager();
        SetupBootstrapUI();

        Debug.Log("[Iteration 1] Bootstrap scene setup complete! Don't forget to create MainMenu and Game scenes and add all 3 scenes to Build Settings.");
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    private static void SetupMusicManager()
    {
        var go = FindOrCreate("MusicManager");
        EnsureComponent<MusicManager>(go);
    }

    private static void SetupSFXManager()
    {
        var go = FindOrCreate("SFXManager");
        EnsureComponent<SFXManager>(go);
    }

    private static void SetupHapticManager()
    {
        var go = FindOrCreate("HapticManager");
        EnsureComponent<HapticManager>(go);
    }

    private static void SetupAddressableLoader()
    {
        var go = FindOrCreate("AddressableLoader");
        EnsureComponent<AddressableLoader>(go);
    }

    private static void SetupSceneLoader()
    {
        var go = FindOrCreate("SceneLoader");
        var sceneLoader = EnsureComponent<SceneLoader>(go);

        var fadeCanvasGo = FindOrCreateChild(go, "FadeCanvas");
        var fadeCanvas = EnsureComponent<Canvas>(fadeCanvasGo);
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;
        var fadeScaler = EnsureComponent<CanvasScaler>(fadeCanvasGo);
        fadeScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        fadeScaler.referenceResolution = new Vector2(1080, 1920);
        fadeScaler.matchWidthOrHeight = 0.5f;
        EnsureComponent<GraphicRaycaster>(fadeCanvasGo);

        var fadePanelGo = FindOrCreateChild(fadeCanvasGo, "FadePanel");
        var fadeImage = EnsureComponent<Image>(fadePanelGo);
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = true;
        StretchFull(fadePanelGo);
        var fadeCG = EnsureComponent<CanvasGroup>(fadePanelGo);
        fadeCG.alpha = 0f;
        fadeCG.blocksRaycasts = false;

        var so = new SerializedObject(sceneLoader);
        so.FindProperty("fadeCanvasGroup").objectReferenceValue = fadeCG;
        so.ApplyModifiedProperties();
    }

    private static void SetupBootstrapUI()
    {
        var canvasGo = FindOrCreate("BootstrapCanvas");
        var canvas = EnsureComponent<Canvas>(canvasGo);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        var scaler = EnsureComponent<CanvasScaler>(canvasGo);
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;
        EnsureComponent<GraphicRaycaster>(canvasGo);

        var bootstrapUI = EnsureComponent<BootstrapUI>(canvasGo);

        var titleGo = FindOrCreateChild(canvasGo, "TitleText");
        var titleTMP = EnsureComponent<TextMeshProUGUI>(titleGo);
        titleTMP.text = "BalloonGame";
        titleTMP.fontSize = 72;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = Color.white;
        titleTMP.fontStyle = FontStyles.Bold;
        var titleRect = titleGo.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.65f);
        titleRect.anchorMax = new Vector2(1, 0.75f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        var progressBgGo = FindOrCreateChild(canvasGo, "ProgressBarBg");
        var progressBgImage = EnsureComponent<Image>(progressBgGo);
        progressBgImage.color = new Color(0.15f, 0.15f, 0.2f);
        var progressBgRect = progressBgGo.GetComponent<RectTransform>();
        progressBgRect.anchorMin = new Vector2(0.15f, 0.485f);
        progressBgRect.anchorMax = new Vector2(0.85f, 0.5f);
        progressBgRect.offsetMin = Vector2.zero;
        progressBgRect.offsetMax = Vector2.zero;

        var progressFillGo = FindOrCreateChild(progressBgGo, "ProgressBarFill");
        var progressFillImage = EnsureComponent<Image>(progressFillGo);
        progressFillImage.color = new Color(0.85f, 0.85f, 0.95f);
        progressFillImage.type = Image.Type.Filled;
        progressFillImage.fillMethod = Image.FillMethod.Horizontal;
        progressFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        progressFillImage.fillAmount = 0f;
        StretchFull(progressFillGo);

        var statusGo = FindOrCreateChild(canvasGo, "StatusText");
        var statusTMP = EnsureComponent<TextMeshProUGUI>(statusGo);
        statusTMP.text = "";
        statusTMP.fontSize = 30;
        statusTMP.alignment = TextAlignmentOptions.Center;
        statusTMP.color = Color.white;
        var statusRect = statusGo.GetComponent<RectTransform>();
        statusRect.anchorMin = new Vector2(0.1f, 0.43f);
        statusRect.anchorMax = new Vector2(0.9f, 0.48f);
        statusRect.offsetMin = Vector2.zero;
        statusRect.offsetMax = Vector2.zero;

        var retryGo = FindOrCreateChild(canvasGo, "RetryButton");
        var retryImage = EnsureComponent<Image>(retryGo);
        retryImage.color = new Color(0.25f, 0.25f, 0.35f);
        var retryButton = EnsureComponent<Button>(retryGo);
        var retryColors = retryButton.colors;
        retryColors.highlightedColor = new Color(0.35f, 0.35f, 0.5f);
        retryColors.pressedColor = new Color(0.2f, 0.2f, 0.3f);
        retryButton.colors = retryColors;
        var retryRect = retryGo.GetComponent<RectTransform>();
        retryRect.anchorMin = new Vector2(0.3f, 0.34f);
        retryRect.anchorMax = new Vector2(0.7f, 0.4f);
        retryRect.offsetMin = Vector2.zero;
        retryRect.offsetMax = Vector2.zero;

        var retryTextGo = FindOrCreateChild(retryGo, "RetryText");
        var retryTMP = EnsureComponent<TextMeshProUGUI>(retryTextGo);
        retryTMP.text = "RETRY";
        retryTMP.fontSize = 36;
        retryTMP.alignment = TextAlignmentOptions.Center;
        retryTMP.color = Color.white;
        retryTMP.fontStyle = FontStyles.Bold;
        StretchFull(retryTextGo);

        var uiSO = new SerializedObject(bootstrapUI);
        uiSO.FindProperty("titleText").objectReferenceValue = titleTMP;
        uiSO.FindProperty("progressFill").objectReferenceValue = progressFillImage;
        uiSO.FindProperty("statusText").objectReferenceValue = statusTMP;
        uiSO.FindProperty("retryButton").objectReferenceValue = retryButton;
        uiSO.ApplyModifiedProperties();
    }

    private static GameObject FindOrCreate(string name)
    {
        var go = GameObject.Find(name);
        if (go == null)
            go = new GameObject(name);
        return go;
    }

    private static GameObject FindOrCreateChild(GameObject parent, string name)
    {
        var t = parent.transform.Find(name);
        if (t != null) return t.gameObject;
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    private static T EnsureComponent<T>(GameObject go) where T : Component
    {
        var comp = go.GetComponent<T>();
        if (comp == null) comp = go.AddComponent<T>();
        return comp;
    }

    private static void StretchFull(GameObject go)
    {
        var rect = go.GetComponent<RectTransform>();
        if (rect == null) return;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
