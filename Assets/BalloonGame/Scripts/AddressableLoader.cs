using UnityEngine;
using System;
using System.Collections;

public class AddressableLoader : MonoBehaviour
{
    public static AddressableLoader Instance { get; private set; }

    public event Action<float> OnDownloadProgress;
    public event Action<string> OnStatusChanged;
    public event Action OnDownloadComplete;
    public event Action<string> OnDownloadFailed;

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

    public void StartLoading()
    {
        StartCoroutine(FakeLoadSequence());
    }

    private IEnumerator FakeLoadSequence()
    {
        OnStatusChanged?.Invoke("Loading...");

        string[] statuses = { "Initializing...", "Checking resources...", "Preparing...", "Ready!" };
        float[] targets = { 0.25f, 0.5f, 0.8f, 1f };

        for (int i = 0; i < statuses.Length; i++)
        {
            OnStatusChanged?.Invoke(statuses[i]);
            float start = i > 0 ? targets[i - 1] : 0f;
            float end = targets[i];
            float duration = 0.3f + UnityEngine.Random.Range(0f, 0.2f);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                OnDownloadProgress?.Invoke(Mathf.Lerp(start, end, t));
                yield return null;
            }
        }

        OnDownloadProgress?.Invoke(1f);
        yield return new WaitForSeconds(0.3f);
        OnDownloadComplete?.Invoke();
    }
}
