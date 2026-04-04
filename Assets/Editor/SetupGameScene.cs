using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SetupGameScene
{
    [MenuItem("BalloonGame/(Iteration 3) Setup Game Scene")]
    public static void SetupIteration3()
    {
        Setup();
    }

    [MenuItem("BalloonGame/(Iteration 4) Setup Game Scene — Obstacles + GameOver")]
    public static void SetupIteration4()
    {
        Setup();
    }

    [MenuItem("BalloonGame/(Iteration 5) Setup Game Scene — Patterns + Difficulty")]
    public static void SetupIteration5()
    {
        Setup();
    }

    [MenuItem("BalloonGame/(Iteration 6) Setup Game Scene — VFX + Game Feel")]
    public static void SetupIteration6()
    {
        Setup();
    }

    [MenuItem("BalloonGame/(Iteration 7) Setup Bootstrap — Audio + Haptics")]
    public static void SetupIteration7Bootstrap()
    {
        SetupBootstrapScene.Setup();
    }

    [MenuItem("BalloonGame/(Iteration 8) Setup Game Scene — Background + TapToStart")]
    public static void SetupIteration8()
    {
        Setup();
    }

    [MenuItem("BalloonGame/(Iteration 9) Setup Game Scene — Bounds + DeathFX + Polish")]
    public static void SetupIteration9()
    {
        Setup();
    }

    public static void Setup()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGo.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        var circleSprite = GetOrCreateCircleSprite("BalloonCircle", 128);
        var shieldSprite = GetOrCreateCircleSprite("ShieldCircle", 128);
        var squareSprite = GetOrCreateSquareSprite("ObstacleSquare", 128);
        var rectSprite = GetOrCreateSquareSprite("ObstacleRect", 128);
        var obstacleCircleSprite = GetOrCreateCircleSprite("ObstacleCircle", 128);

        var cam = SetupCamera();
        var balloon = SetupBalloon(circleSprite);
        var shield = SetupShield(shieldSprite);
        SetupCameraTarget(cam, balloon.transform);
        var spawner = SetupObstacleSpawner(squareSprite, rectSprite, obstacleCircleSprite);
        SetupDifficultyManager();
        SetupParticleManager();
        SetupBackground();
        SetupWorldBounds();
        SetupDeathSequence();
        SetupGameCanvas(balloon, shield);

        Debug.Log("[Iteration 9] Game scene setup complete! Bounds + DeathFX + Polish added.");
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    private static Camera SetupCamera()
    {
        var cam = Camera.main;
        if (cam == null)
        {
            var camGo = new GameObject("Main Camera");
            cam = camGo.AddComponent<Camera>();
            camGo.tag = "MainCamera";
        }
        cam.backgroundColor = new Color(0.06f, 0.06f, 0.1f);
        cam.orthographic = true;
        cam.orthographicSize = 10f;
        EnsureComponent<GameCamera>(cam.gameObject);
        EnsureComponent<ScreenShake>(cam.gameObject);
        return cam;
    }

    private static void SetupCameraTarget(Camera cam, Transform target)
    {
        var gameCam = cam.GetComponent<GameCamera>();
        var so = new SerializedObject(gameCam);
        so.FindProperty("target").objectReferenceValue = target;
        so.ApplyModifiedProperties();
    }

    private static Balloon SetupBalloon(Sprite sprite)
    {
        var go = FindOrCreate("Balloon");
        go.transform.position = new Vector3(0, -3f, 0);

        var sr = EnsureComponent<SpriteRenderer>(go);
        sr.sprite = sprite;
        sr.color = new Color(1f, 0.45f, 0.5f);
        sr.sortingOrder = 5;
        go.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        var rb = EnsureComponent<Rigidbody2D>(go);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        var col = EnsureComponent<CircleCollider2D>(go);
        col.isTrigger = true;
        col.radius = 0.5f;

        var balloon = EnsureComponent<Balloon>(go);
        EnsureComponent<BalloonTrail>(go);
        return balloon;
    }

    private static Shield SetupShield(Sprite sprite)
    {
        var go = FindOrCreate("Shield");
        go.transform.position = new Vector3(0, -5f, 0);

        var sr = EnsureComponent<SpriteRenderer>(go);
        sr.sprite = sprite;
        sr.color = new Color(0.4f, 0.7f, 1f, 0.7f);
        sr.sortingOrder = 10;
        go.transform.localScale = new Vector3(1.2f, 1.2f, 1f);

        var rb = EnsureComponent<Rigidbody2D>(go);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        var col = EnsureComponent<CircleCollider2D>(go);
        col.isTrigger = false;
        col.radius = 0.5f;

        var shield = EnsureComponent<Shield>(go);
        EnsureComponent<ShieldVisuals>(go);
        return shield;
    }

    private static ObstacleSpawner SetupObstacleSpawner(Sprite square, Sprite rect, Sprite circle)
    {
        var go = FindOrCreate("ObstacleSpawner");
        var spawner = EnsureComponent<ObstacleSpawner>(go);
        var so = new SerializedObject(spawner);
        so.FindProperty("squareSprite").objectReferenceValue = square;
        so.FindProperty("rectSprite").objectReferenceValue = rect;
        so.FindProperty("circleSprite").objectReferenceValue = circle;
        so.ApplyModifiedProperties();
        return spawner;
    }

    private static void SetupDifficultyManager()
    {
        var go = FindOrCreate("DifficultyManager");
        EnsureComponent<DifficultyManager>(go);
    }

    private static void SetupParticleManager()
    {
        var go = FindOrCreate("ParticleManager");
        EnsureComponent<ParticleManager>(go);
    }

    private static void SetupBackground()
    {
        var bgGo = FindOrCreate("ScrollingBackground");
        EnsureComponent<ScrollingBackground>(bgGo);

        var gridGo = FindOrCreate("BackgroundGrid");
        EnsureComponent<BackgroundGrid>(gridGo);
    }

    private static void SetupWorldBounds()
    {
        var go = FindOrCreate("WorldBounds");
        EnsureComponent<WorldBounds>(go);
    }

    private static void SetupDeathSequence()
    {
        var go = FindOrCreate("DeathSequence");
        EnsureComponent<DeathSequence>(go);
    }

    private static void SetupGameCanvas(Balloon balloon, Shield shield)
    {
        var canvasGo = FindOrCreate("GameCanvas");
        var canvas = EnsureComponent<Canvas>(canvasGo);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        var scaler = EnsureComponent<CanvasScaler>(canvasGo);
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;
        EnsureComponent<GraphicRaycaster>(canvasGo);

        var gameUI = EnsureComponent<GameUI>(canvasGo);

        var scoreGo = FindOrCreateChild(canvasGo, "ScoreText");
        var scoreTMP = EnsureComponent<TextMeshProUGUI>(scoreGo);
        scoreTMP.text = "0";
        scoreTMP.fontSize = 72;
        scoreTMP.alignment = TextAlignmentOptions.Center;
        scoreTMP.color = new Color(1f, 1f, 1f, 0.8f);
        scoreTMP.fontStyle = FontStyles.Bold;
        var scoreRect = scoreGo.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0.2f, 0.88f);
        scoreRect.anchorMax = new Vector2(0.8f, 0.96f);
        scoreRect.offsetMin = Vector2.zero;
        scoreRect.offsetMax = Vector2.zero;

        var pauseBtnGo = FindOrCreateChild(canvasGo, "PauseButton");
        var pauseBtnImage = EnsureComponent<Image>(pauseBtnGo);
        pauseBtnImage.color = new Color(0.2f, 0.2f, 0.3f, 0.6f);
        var pauseBtn = EnsureComponent<Button>(pauseBtnGo);
        var pauseColors = pauseBtn.colors;
        pauseColors.highlightedColor = new Color(0.3f, 0.3f, 0.45f, 0.8f);
        pauseColors.pressedColor = new Color(0.15f, 0.15f, 0.25f, 0.8f);
        pauseBtn.colors = pauseColors;
        var pauseBtnRect = pauseBtnGo.GetComponent<RectTransform>();
        pauseBtnRect.anchorMin = new Vector2(0.03f, 0.92f);
        pauseBtnRect.anchorMax = new Vector2(0.13f, 0.97f);
        pauseBtnRect.offsetMin = Vector2.zero;
        pauseBtnRect.offsetMax = Vector2.zero;

        var pauseBtnTextGo = FindOrCreateChild(pauseBtnGo, "PauseText");
        var pauseBtnTMP = EnsureComponent<TextMeshProUGUI>(pauseBtnTextGo);
        pauseBtnTMP.text = "| |";
        pauseBtnTMP.fontSize = 32;
        pauseBtnTMP.alignment = TextAlignmentOptions.Center;
        pauseBtnTMP.color = Color.white;
        pauseBtnTMP.fontStyle = FontStyles.Bold;
        StretchFull(pauseBtnTextGo);

        var pausePopup = SetupPausePopup(canvasGo);
        var gameOverPopup = SetupGameOverPopup(canvasGo);

        var tapTextGo = FindOrCreateChild(canvasGo, "TapToStartText");
        var tapTMP = EnsureComponent<TextMeshProUGUI>(tapTextGo);
        tapTMP.text = "TAP TO START";
        tapTMP.fontSize = 48;
        tapTMP.alignment = TextAlignmentOptions.Center;
        tapTMP.color = new Color(1f, 1f, 1f, 0.7f);
        tapTMP.fontStyle = FontStyles.Bold;
        var tapRect = tapTextGo.GetComponent<RectTransform>();
        tapRect.anchorMin = new Vector2(0.1f, 0.35f);
        tapRect.anchorMax = new Vector2(0.9f, 0.45f);
        tapRect.offsetMin = Vector2.zero;
        tapRect.offsetMax = Vector2.zero;

        var tapToStartGo = FindOrCreate("TapToStart");
        var tapToStart = EnsureComponent<TapToStart>(tapToStartGo);
        var tapSO = new SerializedObject(tapToStart);
        tapSO.FindProperty("tapText").objectReferenceValue = tapTMP;
        tapSO.ApplyModifiedProperties();

        var uiSO = new SerializedObject(gameUI);
        uiSO.FindProperty("scoreText").objectReferenceValue = scoreTMP;
        uiSO.FindProperty("pauseButton").objectReferenceValue = pauseBtn;
        uiSO.FindProperty("pausePopup").objectReferenceValue = pausePopup;
        uiSO.ApplyModifiedProperties();

        var gm = FindOrCreate("GameplayManager").GetComponent<GameplayManager>();
        if (gm != null)
        {
            var gmSO = new SerializedObject(gm);
            gmSO.FindProperty("balloon").objectReferenceValue = balloon;
            gmSO.FindProperty("shield").objectReferenceValue = shield;
            gmSO.FindProperty("gameOverPopup").objectReferenceValue = gameOverPopup;
            gmSO.ApplyModifiedProperties();
        }
    }

    private static PausePopup SetupPausePopup(GameObject canvasGo)
    {
        var popupGo = FindOrCreateChild(canvasGo, "PausePopup");
        EnsureRectTransform(popupGo);
        StretchFull(popupGo);
        var pausePopup = EnsureComponent<PausePopup>(popupGo);

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

        var pausedTitleGo = FindOrCreateChild(panelGo, "PausedTitle");
        var pausedTitleTMP = EnsureComponent<TextMeshProUGUI>(pausedTitleGo);
        pausedTitleTMP.text = "PAUSED";
        pausedTitleTMP.fontSize = 56;
        pausedTitleTMP.alignment = TextAlignmentOptions.Center;
        pausedTitleTMP.color = Color.white;
        pausedTitleTMP.fontStyle = FontStyles.Bold;
        var pausedTitleRect = pausedTitleGo.GetComponent<RectTransform>();
        pausedTitleRect.anchorMin = new Vector2(0.05f, 0.7f);
        pausedTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
        pausedTitleRect.offsetMin = Vector2.zero;
        pausedTitleRect.offsetMax = Vector2.zero;

        var resumeBtnGo = FindOrCreateChild(panelGo, "ResumeButton");
        var resumeBtnImage = EnsureComponent<Image>(resumeBtnGo);
        resumeBtnImage.color = new Color(0.3f, 0.7f, 0.4f);
        var resumeBtn = EnsureComponent<Button>(resumeBtnGo);
        var resumeBtnRect = resumeBtnGo.GetComponent<RectTransform>();
        resumeBtnRect.anchorMin = new Vector2(0.15f, 0.4f);
        resumeBtnRect.anchorMax = new Vector2(0.85f, 0.6f);
        resumeBtnRect.offsetMin = Vector2.zero;
        resumeBtnRect.offsetMax = Vector2.zero;

        var resumeTextGo = FindOrCreateChild(resumeBtnGo, "ResumeText");
        var resumeTMP = EnsureComponent<TextMeshProUGUI>(resumeTextGo);
        resumeTMP.text = "RESUME";
        resumeTMP.fontSize = 40;
        resumeTMP.alignment = TextAlignmentOptions.Center;
        resumeTMP.color = Color.white;
        resumeTMP.fontStyle = FontStyles.Bold;
        StretchFull(resumeTextGo);

        var menuBtnGo = FindOrCreateChild(panelGo, "MenuButton");
        var menuBtnImage = EnsureComponent<Image>(menuBtnGo);
        menuBtnImage.color = new Color(0.6f, 0.25f, 0.25f);
        var menuBtn = EnsureComponent<Button>(menuBtnGo);
        var menuBtnRect = menuBtnGo.GetComponent<RectTransform>();
        menuBtnRect.anchorMin = new Vector2(0.15f, 0.1f);
        menuBtnRect.anchorMax = new Vector2(0.85f, 0.3f);
        menuBtnRect.offsetMin = Vector2.zero;
        menuBtnRect.offsetMax = Vector2.zero;

        var menuTextGo = FindOrCreateChild(menuBtnGo, "MenuText");
        var menuTMP = EnsureComponent<TextMeshProUGUI>(menuTextGo);
        menuTMP.text = "MENU";
        menuTMP.fontSize = 40;
        menuTMP.alignment = TextAlignmentOptions.Center;
        menuTMP.color = Color.white;
        menuTMP.fontStyle = FontStyles.Bold;
        StretchFull(menuTextGo);

        var popupSO = new SerializedObject(pausePopup);
        popupSO.FindProperty("dimBg").objectReferenceValue = dimBgCG;
        popupSO.FindProperty("panel").objectReferenceValue = panelRect;
        popupSO.FindProperty("resumeButton").objectReferenceValue = resumeBtn;
        popupSO.FindProperty("menuButton").objectReferenceValue = menuBtn;
        popupSO.ApplyModifiedProperties();

        popupGo.SetActive(false);
        return pausePopup;
    }

    private static GameOverPopup SetupGameOverPopup(GameObject canvasGo)
    {
        var popupGo = FindOrCreateChild(canvasGo, "GameOverPopup");
        EnsureRectTransform(popupGo);
        StretchFull(popupGo);
        var gameOverPopup = EnsureComponent<GameOverPopup>(popupGo);

        var dimBgGo = FindOrCreateChild(popupGo, "DimBg");
        var dimBgImage = EnsureComponent<Image>(dimBgGo);
        dimBgImage.color = new Color(0, 0, 0, 0.7f);
        dimBgImage.raycastTarget = true;
        StretchFull(dimBgGo);
        var dimBgCG = EnsureComponent<CanvasGroup>(dimBgGo);

        var panelGo = FindOrCreateChild(popupGo, "Panel");
        var panelImage = EnsureComponent<Image>(panelGo);
        panelImage.color = new Color(0.1f, 0.1f, 0.15f);
        panelImage.raycastTarget = true;
        var panelRect = panelGo.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.08f, 0.25f);
        panelRect.anchorMax = new Vector2(0.92f, 0.75f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var gameOverTitleGo = FindOrCreateChild(panelGo, "GameOverTitle");
        var gameOverTitleTMP = EnsureComponent<TextMeshProUGUI>(gameOverTitleGo);
        gameOverTitleTMP.text = "GAME OVER";
        gameOverTitleTMP.fontSize = 56;
        gameOverTitleTMP.alignment = TextAlignmentOptions.Center;
        gameOverTitleTMP.color = new Color(1f, 0.4f, 0.4f);
        gameOverTitleTMP.fontStyle = FontStyles.Bold;
        var goTitleRect = gameOverTitleGo.GetComponent<RectTransform>();
        goTitleRect.anchorMin = new Vector2(0.05f, 0.82f);
        goTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
        goTitleRect.offsetMin = Vector2.zero;
        goTitleRect.offsetMax = Vector2.zero;

        var scoreLabelGo = FindOrCreateChild(panelGo, "ScoreLabel");
        var scoreLabelTMP = EnsureComponent<TextMeshProUGUI>(scoreLabelGo);
        scoreLabelTMP.text = "SCORE";
        scoreLabelTMP.fontSize = 30;
        scoreLabelTMP.alignment = TextAlignmentOptions.Center;
        scoreLabelTMP.color = new Color(0.6f, 0.6f, 0.7f);
        var scoreLabelRect = scoreLabelGo.GetComponent<RectTransform>();
        scoreLabelRect.anchorMin = new Vector2(0.1f, 0.68f);
        scoreLabelRect.anchorMax = new Vector2(0.9f, 0.76f);
        scoreLabelRect.offsetMin = Vector2.zero;
        scoreLabelRect.offsetMax = Vector2.zero;

        var scoreValueGo = FindOrCreateChild(panelGo, "ScoreValue");
        var scoreValueTMP = EnsureComponent<TextMeshProUGUI>(scoreValueGo);
        scoreValueTMP.text = "0";
        scoreValueTMP.fontSize = 64;
        scoreValueTMP.alignment = TextAlignmentOptions.Center;
        scoreValueTMP.color = Color.white;
        scoreValueTMP.fontStyle = FontStyles.Bold;
        var scoreValRect = scoreValueGo.GetComponent<RectTransform>();
        scoreValRect.anchorMin = new Vector2(0.1f, 0.58f);
        scoreValRect.anchorMax = new Vector2(0.9f, 0.7f);
        scoreValRect.offsetMin = Vector2.zero;
        scoreValRect.offsetMax = Vector2.zero;

        var bestLabelGo = FindOrCreateChild(panelGo, "BestLabel");
        var bestLabelTMP = EnsureComponent<TextMeshProUGUI>(bestLabelGo);
        bestLabelTMP.text = "BEST";
        bestLabelTMP.fontSize = 26;
        bestLabelTMP.alignment = TextAlignmentOptions.Center;
        bestLabelTMP.color = new Color(0.6f, 0.6f, 0.7f);
        var bestLabelRect = bestLabelGo.GetComponent<RectTransform>();
        bestLabelRect.anchorMin = new Vector2(0.1f, 0.48f);
        bestLabelRect.anchorMax = new Vector2(0.9f, 0.55f);
        bestLabelRect.offsetMin = Vector2.zero;
        bestLabelRect.offsetMax = Vector2.zero;

        var bestValueGo = FindOrCreateChild(panelGo, "BestValue");
        var bestValueTMP = EnsureComponent<TextMeshProUGUI>(bestValueGo);
        bestValueTMP.text = "0";
        bestValueTMP.fontSize = 44;
        bestValueTMP.alignment = TextAlignmentOptions.Center;
        bestValueTMP.color = new Color(0.8f, 0.8f, 0.9f);
        bestValueTMP.fontStyle = FontStyles.Bold;
        var bestValRect = bestValueGo.GetComponent<RectTransform>();
        bestValRect.anchorMin = new Vector2(0.1f, 0.4f);
        bestValRect.anchorMax = new Vector2(0.9f, 0.5f);
        bestValRect.offsetMin = Vector2.zero;
        bestValRect.offsetMax = Vector2.zero;

        var newBestGo = FindOrCreateChild(panelGo, "NewBestText");
        var newBestTMP = EnsureComponent<TextMeshProUGUI>(newBestGo);
        newBestTMP.text = "NEW BEST!";
        newBestTMP.fontSize = 36;
        newBestTMP.alignment = TextAlignmentOptions.Center;
        newBestTMP.color = new Color(1f, 0.85f, 0.2f);
        newBestTMP.fontStyle = FontStyles.Bold;
        var newBestRect = newBestGo.GetComponent<RectTransform>();
        newBestRect.anchorMin = new Vector2(0.1f, 0.33f);
        newBestRect.anchorMax = new Vector2(0.9f, 0.4f);
        newBestRect.offsetMin = Vector2.zero;
        newBestRect.offsetMax = Vector2.zero;
        newBestGo.SetActive(false);

        var restartBtnGo = FindOrCreateChild(panelGo, "RestartButton");
        var restartBtnImage = EnsureComponent<Image>(restartBtnGo);
        restartBtnImage.color = new Color(0.3f, 0.7f, 0.4f);
        var restartBtn = EnsureComponent<Button>(restartBtnGo);
        var restartBtnRect = restartBtnGo.GetComponent<RectTransform>();
        restartBtnRect.anchorMin = new Vector2(0.15f, 0.15f);
        restartBtnRect.anchorMax = new Vector2(0.85f, 0.28f);
        restartBtnRect.offsetMin = Vector2.zero;
        restartBtnRect.offsetMax = Vector2.zero;

        var restartTextGo = FindOrCreateChild(restartBtnGo, "RestartText");
        var restartTMP = EnsureComponent<TextMeshProUGUI>(restartTextGo);
        restartTMP.text = "RESTART";
        restartTMP.fontSize = 40;
        restartTMP.alignment = TextAlignmentOptions.Center;
        restartTMP.color = Color.white;
        restartTMP.fontStyle = FontStyles.Bold;
        StretchFull(restartTextGo);

        var menuBtnGo = FindOrCreateChild(panelGo, "MenuButton");
        var menuBtnImage = EnsureComponent<Image>(menuBtnGo);
        menuBtnImage.color = new Color(0.6f, 0.25f, 0.25f);
        var menuBtn = EnsureComponent<Button>(menuBtnGo);
        var menuBtnRect = menuBtnGo.GetComponent<RectTransform>();
        menuBtnRect.anchorMin = new Vector2(0.15f, 0.03f);
        menuBtnRect.anchorMax = new Vector2(0.85f, 0.13f);
        menuBtnRect.offsetMin = Vector2.zero;
        menuBtnRect.offsetMax = Vector2.zero;

        var menuTextGo = FindOrCreateChild(menuBtnGo, "MenuText");
        var menuTMP = EnsureComponent<TextMeshProUGUI>(menuTextGo);
        menuTMP.text = "MENU";
        menuTMP.fontSize = 40;
        menuTMP.alignment = TextAlignmentOptions.Center;
        menuTMP.color = Color.white;
        menuTMP.fontStyle = FontStyles.Bold;
        StretchFull(menuTextGo);

        var popupSO = new SerializedObject(gameOverPopup);
        popupSO.FindProperty("dimBg").objectReferenceValue = dimBgCG;
        popupSO.FindProperty("panel").objectReferenceValue = panelRect;
        popupSO.FindProperty("scoreValueText").objectReferenceValue = scoreValueTMP;
        popupSO.FindProperty("bestValueText").objectReferenceValue = bestValueTMP;
        popupSO.FindProperty("newBestText").objectReferenceValue = newBestTMP;
        popupSO.FindProperty("restartButton").objectReferenceValue = restartBtn;
        popupSO.FindProperty("menuButton").objectReferenceValue = menuBtn;
        popupSO.ApplyModifiedProperties();

        popupGo.SetActive(false);
        return gameOverPopup;
    }

    private static Sprite GetOrCreateCircleSprite(string name, int size)
    {
        string dir = "Assets/BalloonGame/Sprites";
        string path = $"{dir}/{name}.png";
        var existing = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (existing != null) return existing;

        EnsureSpriteDir(dir);

        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float center = size / 2f;
        float radius = size / 2f - 1f;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist <= radius - 1f) tex.SetPixel(x, y, Color.white);
                else if (dist <= radius) tex.SetPixel(x, y, new Color(1, 1, 1, radius - dist));
                else tex.SetPixel(x, y, Color.clear);
            }
        tex.Apply();
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);
        AssetDatabase.Refresh();
        SetSpriteImportSettings(path, size);
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static Sprite GetOrCreateSquareSprite(string name, int size)
    {
        string dir = "Assets/BalloonGame/Sprites";
        string path = $"{dir}/{name}.png";
        var existing = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (existing != null) return existing;

        EnsureSpriteDir(dir);

        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);
        AssetDatabase.Refresh();
        SetSpriteImportSettings(path, size);
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static void EnsureSpriteDir(string dir)
    {
        if (!AssetDatabase.IsValidFolder(dir))
        {
            if (!AssetDatabase.IsValidFolder("Assets/BalloonGame"))
                AssetDatabase.CreateFolder("Assets", "BalloonGame");
            AssetDatabase.CreateFolder("Assets/BalloonGame", "Sprites");
        }
    }

    private static void SetSpriteImportSettings(string path, int ppu)
    {
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = ppu;
            importer.filterMode = FilterMode.Bilinear;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }
    }

    private static GameObject FindOrCreate(string name)
    {
        var go = GameObject.Find(name);
        if (go == null) go = new GameObject(name);
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
        if (rect == null) rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
