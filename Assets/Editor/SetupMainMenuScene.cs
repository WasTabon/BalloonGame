using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
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
        SetupMenuBackground();
        SetupMainMenuCanvas();

        Debug.Log("[Iteration 9] MainMenu scene setup complete!");
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    private static void SetupMenuBackground()
    {
        var bgGo = FindOrCreate("MenuBackground");
        EnsureComponent<ScrollingBackground>(bgGo);

        var decorGo = FindOrCreate("DecorBalloon");
        var decorSR = EnsureComponent<SpriteRenderer>(decorGo);
        decorSR.color = new Color(1f, 0.45f, 0.5f, 0.15f);
        decorSR.sortingOrder = -5;

        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BalloonGame/Sprites/BalloonCircle.png");
        if (sprite != null)
            decorSR.sprite = sprite;

        decorGo.transform.position = new Vector3(0, -1f, 0);
        decorGo.transform.localScale = new Vector3(4f, 4f, 1f);
        EnsureComponent<DecorBalloon>(decorGo);
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
        var modeSelectPopup = SetupModeSelectPopup(canvasGo);

        var menuSO = new SerializedObject(menuUI);
        menuSO.FindProperty("titleText").objectReferenceValue = titleTMP;
        menuSO.FindProperty("bestScoreText").objectReferenceValue = bestScoreTMP;
        menuSO.FindProperty("playButton").objectReferenceValue = playButton;
        menuSO.FindProperty("settingsButton").objectReferenceValue = settingsBtn;
        menuSO.FindProperty("settingsPopup").objectReferenceValue = settingsPopup;
        menuSO.FindProperty("modeSelectPopup").objectReferenceValue = modeSelectPopup;
        menuSO.ApplyModifiedProperties();
    }

    private static ModeSelectPopup SetupModeSelectPopup(GameObject canvasGo)
    {
        var popupGo = FindOrCreateChild(canvasGo, "ModeSelectPopup");
        EnsureRectTransform(popupGo);
        StretchFull(popupGo);
        var modePopup = EnsureComponent<ModeSelectPopup>(popupGo);

        var dimBgGo = FindOrCreateChild(popupGo, "DimBg");
        var dimBgImage = EnsureComponent<Image>(dimBgGo);
        dimBgImage.color = new Color(0, 0, 0, 0.6f);
        dimBgImage.raycastTarget = true;
        StretchFull(dimBgGo);
        var dimBgCG = EnsureComponent<CanvasGroup>(dimBgGo);

        var panelGo = FindOrCreateChild(popupGo, "Panel");
        var panelImage = EnsureComponent<Image>(panelGo);
        panelImage.color = new Color(0.1f, 0.1f, 0.15f);
        panelImage.raycastTarget = true;
        var panelRect = panelGo.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.08f, 0.2f);
        panelRect.anchorMax = new Vector2(0.92f, 0.8f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var titleGo = FindOrCreateChild(panelGo, "ModeTitle");
        var titleTMP = EnsureComponent<TextMeshProUGUI>(titleGo);
        titleTMP.text = "SELECT MODE";
        titleTMP.fontSize = 48;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = Color.white;
        titleTMP.fontStyle = FontStyles.Bold;
        var titleRect = titleGo.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.05f, 0.85f);
        titleRect.anchorMax = new Vector2(0.95f, 0.97f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        var classicBtn = CreateModeButton(panelGo, "ClassicButton", "CLASSIC",
            "Survive as long as you can", new Color(0.3f, 0.7f, 0.4f),
            new Vector2(0.1f, 0.62f), new Vector2(0.9f, 0.82f), out var classicBestTMP);

        var timeAttackBtn = CreateModeButton(panelGo, "TimeAttackButton", "TIME ATTACK",
            "30 seconds — max score!", new Color(0.9f, 0.5f, 0.2f),
            new Vector2(0.1f, 0.38f), new Vector2(0.9f, 0.58f), out var timeAttackBestTMP);

        var shieldRushBtn = CreateModeButton(panelGo, "ShieldRushButton", "SHIELD RUSH",
            "Big shield, tons of chaos!", new Color(0.4f, 0.5f, 0.9f),
            new Vector2(0.1f, 0.14f), new Vector2(0.9f, 0.34f), out var shieldRushBestTMP);

        var closeBtnGo = FindOrCreateChild(panelGo, "CloseButton");
        var closeBtnImage = EnsureComponent<Image>(closeBtnGo);
        closeBtnImage.color = new Color(0.4f, 0.4f, 0.5f);
        var closeBtn = EnsureComponent<Button>(closeBtnGo);
        var closeBtnRect = closeBtnGo.GetComponent<RectTransform>();
        closeBtnRect.anchorMin = new Vector2(0.8f, 0.88f);
        closeBtnRect.anchorMax = new Vector2(0.95f, 0.97f);
        closeBtnRect.offsetMin = Vector2.zero;
        closeBtnRect.offsetMax = Vector2.zero;
        var closeTextGo = FindOrCreateChild(closeBtnGo, "CloseText");
        var closeTMP = EnsureComponent<TextMeshProUGUI>(closeTextGo);
        closeTMP.text = "X";
        closeTMP.fontSize = 32;
        closeTMP.alignment = TextAlignmentOptions.Center;
        closeTMP.color = Color.white;
        closeTMP.fontStyle = FontStyles.Bold;
        StretchFull(closeTextGo);

        var popupSO = new SerializedObject(modePopup);
        popupSO.FindProperty("dimBg").objectReferenceValue = dimBgCG;
        popupSO.FindProperty("panel").objectReferenceValue = panelRect;
        popupSO.FindProperty("classicButton").objectReferenceValue = classicBtn;
        popupSO.FindProperty("timeAttackButton").objectReferenceValue = timeAttackBtn;
        popupSO.FindProperty("shieldRushButton").objectReferenceValue = shieldRushBtn;
        popupSO.FindProperty("closeButton").objectReferenceValue = closeBtn;
        popupSO.FindProperty("classicBestText").objectReferenceValue = classicBestTMP;
        popupSO.FindProperty("timeAttackBestText").objectReferenceValue = timeAttackBestTMP;
        popupSO.FindProperty("shieldRushBestText").objectReferenceValue = shieldRushBestTMP;
        popupSO.ApplyModifiedProperties();

        popupGo.SetActive(false);
        return modePopup;
    }

    private static Button CreateModeButton(GameObject parent, string name, string modeName, string desc,
        Color color, Vector2 anchorMin, Vector2 anchorMax, out TextMeshProUGUI bestText)
    {
        var btnGo = FindOrCreateChild(parent, name);
        var btnImage = EnsureComponent<Image>(btnGo);
        btnImage.color = color;
        var btn = EnsureComponent<Button>(btnGo);
        var btnColors = btn.colors;
        btnColors.highlightedColor = color * 1.2f;
        btnColors.pressedColor = color * 0.8f;
        btn.colors = btnColors;
        var btnRect = btnGo.GetComponent<RectTransform>();
        btnRect.anchorMin = anchorMin;
        btnRect.anchorMax = anchorMax;
        btnRect.offsetMin = Vector2.zero;
        btnRect.offsetMax = Vector2.zero;

        var nameGo = FindOrCreateChild(btnGo, "ModeName");
        var nameTMP = EnsureComponent<TextMeshProUGUI>(nameGo);
        nameTMP.text = modeName;
        nameTMP.fontSize = 36;
        nameTMP.alignment = TextAlignmentOptions.Left;
        nameTMP.color = Color.white;
        nameTMP.fontStyle = FontStyles.Bold;
        var nameRect = nameGo.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0.05f, 0.5f);
        nameRect.anchorMax = new Vector2(0.7f, 0.95f);
        nameRect.offsetMin = Vector2.zero;
        nameRect.offsetMax = Vector2.zero;

        var descGo = FindOrCreateChild(btnGo, "ModeDesc");
        var descTMP = EnsureComponent<TextMeshProUGUI>(descGo);
        descTMP.text = desc;
        descTMP.fontSize = 22;
        descTMP.alignment = TextAlignmentOptions.Left;
        descTMP.color = new Color(1, 1, 1, 0.7f);
        var descRect = descGo.GetComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0.05f, 0.05f);
        descRect.anchorMax = new Vector2(0.7f, 0.5f);
        descRect.offsetMin = Vector2.zero;
        descRect.offsetMax = Vector2.zero;

        var bestGo = FindOrCreateChild(btnGo, "BestText");
        bestText = EnsureComponent<TextMeshProUGUI>(bestGo);
        bestText.text = "BEST: 0";
        bestText.fontSize = 24;
        bestText.alignment = TextAlignmentOptions.Right;
        bestText.color = new Color(1, 1, 1, 0.6f);
        var bestRect = bestGo.GetComponent<RectTransform>();
        bestRect.anchorMin = new Vector2(0.65f, 0.3f);
        bestRect.anchorMax = new Vector2(0.95f, 0.7f);
        bestRect.offsetMin = Vector2.zero;
        bestRect.offsetMax = Vector2.zero;

        return btn;
    }

    private static SettingsPopup SetupSettingsPopup(GameObject canvasGo)
    {
        var popupGo = FindOrCreateChild(canvasGo, "SettingsPopup");
        EnsureRectTransform(popupGo);
        StretchFull(popupGo);
        var settingsPopup = EnsureComponent<SettingsPopup>(popupGo);

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

    private static void EnsureRectTransform(GameObject go)
    {
        if (go.GetComponent<RectTransform>() == null)
            go.AddComponent<RectTransform>();
    }

    private static void StretchFull(GameObject go)
    {
        var rect = go.GetComponent<RectTransform>();
        if (rect == null)
            rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
