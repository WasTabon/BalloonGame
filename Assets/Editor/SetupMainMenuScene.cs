using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class SetupMainMenuScene
{
    [MenuItem("BalloonGame/(Iteration 2) Setup MainMenu Scene")]
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

        SetupGameManager();
        SetupMainMenuCanvas();

        Debug.Log("[Iteration 2] MainMenu scene setup complete!");
    }

    private static void SetupGameManager()
    {
        var go = FindOrCreate("GameManager");
        EnsureComponent<GameManager>(go);
    }

    private static void SetupMainMenuCanvas()
    {
        var canvasGo = FindOrCreate("MainMenuCanvas");
        var canvas = EnsureComponent<Canvas>(canvasGo);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        var scaler = EnsureComponent<CanvasScaler>(canvasGo);
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;
        EnsureComponent<GraphicRaycaster>(canvasGo);

        var menuUI = EnsureComponent<MainMenuUI>(canvasGo);

        var titleGo = FindOrCreateChild(canvasGo, "TitleText");
        var titleTMP = EnsureComponent<TextMeshProUGUI>(titleGo);
        titleTMP.text = "BalloonGame";
        titleTMP.fontSize = 80;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = Color.white;
        titleTMP.fontStyle = FontStyles.Bold;
        var titleRect = titleGo.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.72f);
        titleRect.anchorMax = new Vector2(1, 0.82f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        var bestScoreGo = FindOrCreateChild(canvasGo, "BestScoreText");
        var bestScoreTMP = EnsureComponent<TextMeshProUGUI>(bestScoreGo);
        bestScoreTMP.text = "BEST: 0";
        bestScoreTMP.fontSize = 36;
        bestScoreTMP.alignment = TextAlignmentOptions.Center;
        bestScoreTMP.color = new Color(0.7f, 0.7f, 0.8f);
        var bestRect = bestScoreGo.GetComponent<RectTransform>();
        bestRect.anchorMin = new Vector2(0, 0.64f);
        bestRect.anchorMax = new Vector2(1, 0.7f);
        bestRect.offsetMin = Vector2.zero;
        bestRect.offsetMax = Vector2.zero;

        var playGo = FindOrCreateChild(canvasGo, "PlayButton");
        var playImage = EnsureComponent<Image>(playGo);
        playImage.color = new Color(0.3f, 0.7f, 0.4f);
        var playButton = EnsureComponent<Button>(playGo);
        var playColors = playButton.colors;
        playColors.highlightedColor = new Color(0.35f, 0.8f, 0.45f);
        playColors.pressedColor = new Color(0.25f, 0.6f, 0.35f);
        playButton.colors = playColors;
        var playRect = playGo.GetComponent<RectTransform>();
        playRect.anchorMin = new Vector2(0.25f, 0.43f);
        playRect.anchorMax = new Vector2(0.75f, 0.53f);
        playRect.offsetMin = Vector2.zero;
        playRect.offsetMax = Vector2.zero;

        var playTextGo = FindOrCreateChild(playGo, "PlayText");
        var playTMP = EnsureComponent<TextMeshProUGUI>(playTextGo);
        playTMP.text = "PLAY";
        playTMP.fontSize = 56;
        playTMP.alignment = TextAlignmentOptions.Center;
        playTMP.color = Color.white;
        playTMP.fontStyle = FontStyles.Bold;
        StretchFull(playTextGo);

        var settingsBtnGo = FindOrCreateChild(canvasGo, "SettingsButton");
        var settingsBtnImage = EnsureComponent<Image>(settingsBtnGo);
        settingsBtnImage.color = new Color(0.25f, 0.25f, 0.35f);
        var settingsBtn = EnsureComponent<Button>(settingsBtnGo);
        var settingsColors = settingsBtn.colors;
        settingsColors.highlightedColor = new Color(0.35f, 0.35f, 0.5f);
        settingsColors.pressedColor = new Color(0.2f, 0.2f, 0.3f);
        settingsBtn.colors = settingsColors;
        var settingsBtnRect = settingsBtnGo.GetComponent<RectTransform>();
        settingsBtnRect.anchorMin = new Vector2(0.82f, 0.9f);
        settingsBtnRect.anchorMax = new Vector2(0.95f, 0.96f);
        settingsBtnRect.offsetMin = Vector2.zero;
        settingsBtnRect.offsetMax = Vector2.zero;

        var settingsBtnTextGo = FindOrCreateChild(settingsBtnGo, "SettingsText");
        var settingsBtnTMP = EnsureComponent<TextMeshProUGUI>(settingsBtnTextGo);
        settingsBtnTMP.text = "\u2699";
        settingsBtnTMP.fontSize = 48;
        settingsBtnTMP.alignment = TextAlignmentOptions.Center;
        settingsBtnTMP.color = Color.white;
        StretchFull(settingsBtnTextGo);

        var settingsPopup = SetupSettingsPopup(canvasGo);

        var menuSO = new SerializedObject(menuUI);
        menuSO.FindProperty("titleText").objectReferenceValue = titleTMP;
        menuSO.FindProperty("bestScoreText").objectReferenceValue = bestScoreTMP;
        menuSO.FindProperty("playButton").objectReferenceValue = playButton;
        menuSO.FindProperty("settingsButton").objectReferenceValue = settingsBtn;
        menuSO.FindProperty("settingsPopup").objectReferenceValue = settingsPopup;
        menuSO.ApplyModifiedProperties();
    }

    private static SettingsPopup SetupSettingsPopup(GameObject canvasGo)
    {
        var popupGo = FindOrCreateChild(canvasGo, "SettingsPopup");
        var settingsPopup = EnsureComponent<SettingsPopup>(popupGo);
        StretchFull(popupGo);

        var dimBgGo = FindOrCreateChild(popupGo, "DimBg");
        var dimBgImage = EnsureComponent<Image>(dimBgGo);
        dimBgImage.color = new Color(0, 0, 0, 0.6f);
        dimBgImage.raycastTarget = true;
        StretchFull(dimBgGo);
        var dimBgCG = EnsureComponent<CanvasGroup>(dimBgGo);

        var panelGo = FindOrCreateChild(popupGo, "Panel");
        var panelImage = EnsureComponent<Image>(panelGo);
        panelImage.color = new Color(0.12f, 0.12f, 0.18f);
        panelImage.raycastTarget = true;
        var panelRect = panelGo.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.1f, 0.35f);
        panelRect.anchorMax = new Vector2(0.9f, 0.65f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var settingsTitleGo = FindOrCreateChild(panelGo, "SettingsTitle");
        var settingsTitleTMP = EnsureComponent<TextMeshProUGUI>(settingsTitleGo);
        settingsTitleTMP.text = "SETTINGS";
        settingsTitleTMP.fontSize = 48;
        settingsTitleTMP.alignment = TextAlignmentOptions.Center;
        settingsTitleTMP.color = Color.white;
        settingsTitleTMP.fontStyle = FontStyles.Bold;
        var settingsTitleRect = settingsTitleGo.GetComponent<RectTransform>();
        settingsTitleRect.anchorMin = new Vector2(0.05f, 0.75f);
        settingsTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
        settingsTitleRect.offsetMin = Vector2.zero;
        settingsTitleRect.offsetMax = Vector2.zero;

        var soundBtnGo = FindOrCreateChild(panelGo, "SoundToggleButton");
        var soundBtnImage = EnsureComponent<Image>(soundBtnGo);
        soundBtnImage.color = new Color(0.25f, 0.25f, 0.35f);
        var soundBtn = EnsureComponent<Button>(soundBtnGo);
        var soundColors = soundBtn.colors;
        soundColors.highlightedColor = new Color(0.35f, 0.35f, 0.5f);
        soundColors.pressedColor = new Color(0.2f, 0.2f, 0.3f);
        soundBtn.colors = soundColors;
        var soundBtnRect = soundBtnGo.GetComponent<RectTransform>();
        soundBtnRect.anchorMin = new Vector2(0.15f, 0.45f);
        soundBtnRect.anchorMax = new Vector2(0.85f, 0.65f);
        soundBtnRect.offsetMin = Vector2.zero;
        soundBtnRect.offsetMax = Vector2.zero;

        var soundTextGo = FindOrCreateChild(soundBtnGo, "SoundToggleText");
        var soundTMP = EnsureComponent<TextMeshProUGUI>(soundTextGo);
        soundTMP.text = "SOUND: ON";
        soundTMP.fontSize = 36;
        soundTMP.alignment = TextAlignmentOptions.Center;
        soundTMP.color = Color.white;
        soundTMP.fontStyle = FontStyles.Bold;
        StretchFull(soundTextGo);

        var closeBtnGo = FindOrCreateChild(panelGo, "CloseButton");
        var closeBtnImage = EnsureComponent<Image>(closeBtnGo);
        closeBtnImage.color = new Color(0.6f, 0.25f, 0.25f);
        var closeBtn = EnsureComponent<Button>(closeBtnGo);
        var closeColors = closeBtn.colors;
        closeColors.highlightedColor = new Color(0.7f, 0.3f, 0.3f);
        closeColors.pressedColor = new Color(0.5f, 0.2f, 0.2f);
        closeBtn.colors = closeColors;
        var closeBtnRect = closeBtnGo.GetComponent<RectTransform>();
        closeBtnRect.anchorMin = new Vector2(0.25f, 0.1f);
        closeBtnRect.anchorMax = new Vector2(0.75f, 0.3f);
        closeBtnRect.offsetMin = Vector2.zero;
        closeBtnRect.offsetMax = Vector2.zero;

        var closeTextGo = FindOrCreateChild(closeBtnGo, "CloseText");
        var closeTMP = EnsureComponent<TextMeshProUGUI>(closeTextGo);
        closeTMP.text = "CLOSE";
        closeTMP.fontSize = 36;
        closeTMP.alignment = TextAlignmentOptions.Center;
        closeTMP.color = Color.white;
        closeTMP.fontStyle = FontStyles.Bold;
        StretchFull(closeTextGo);

        var popupSO = new SerializedObject(settingsPopup);
        popupSO.FindProperty("dimBg").objectReferenceValue = dimBgCG;
        popupSO.FindProperty("panel").objectReferenceValue = panelRect;
        popupSO.FindProperty("soundToggleButton").objectReferenceValue = soundBtn;
        popupSO.FindProperty("soundToggleText").objectReferenceValue = soundTMP;
        popupSO.FindProperty("closeButton").objectReferenceValue = closeBtn;
        popupSO.ApplyModifiedProperties();

        popupGo.SetActive(false);

        return settingsPopup;
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
