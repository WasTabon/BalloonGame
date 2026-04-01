using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.4f;

    private bool isLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, Action onSceneLoaded = null)
    {
        if (isLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName, onSceneLoaded));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, Action onSceneLoaded)
    {
        isLoading = true;
        fadeCanvasGroup.blocksRaycasts = true;

        yield return fadeCanvasGroup.DOFade(1f, fadeDuration)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();

        var asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
            yield return null;

        onSceneLoaded?.Invoke();

        yield return fadeCanvasGroup.DOFade(0f, fadeDuration)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();

        fadeCanvasGroup.blocksRaycasts = false;
        isLoading = false;
    }
}
