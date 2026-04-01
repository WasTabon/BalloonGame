using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

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
        StartCoroutine(LoadSequence());
    }

    private IEnumerator LoadSequence()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OnStatusChanged?.Invoke("Loading...");
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f)
        {
            OnDownloadProgress?.Invoke(t);
            yield return null;
        }
        OnDownloadProgress?.Invoke(1f);
        yield return new WaitForSeconds(0.3f);
        OnDownloadComplete?.Invoke();
#else
        OnStatusChanged?.Invoke("Checking connection...");
        OnDownloadProgress?.Invoke(0.05f);
        using (var request = UnityWebRequest.Head("https://google.com"))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                OnDownloadFailed?.Invoke("No internet connection");
                yield break;
            }
        }

        OnStatusChanged?.Invoke("Initializing...");
        OnDownloadProgress?.Invoke(0.1f);
        bool initSuccess = false;
        var initOp = Addressables.InitializeAsync();
        initOp.Completed += handle => { initSuccess = handle.Status == AsyncOperationStatus.Succeeded; };
        yield return initOp;
        if (!initSuccess)
        {
            OnDownloadFailed?.Invoke("Failed to initialize resources");
            yield break;
        }

        OnStatusChanged?.Invoke("Checking for updates...");
        OnDownloadProgress?.Invoke(0.2f);
        bool catalogSuccess = false;
        List<string> catalogsToUpdate = null;
        var catalogOp = Addressables.CheckForCatalogUpdates(false);
        catalogOp.Completed += handle =>
        {
            catalogSuccess = handle.Status == AsyncOperationStatus.Succeeded;
            if (catalogSuccess && handle.Result != null)
                catalogsToUpdate = new List<string>(handle.Result);
        };
        yield return catalogOp;
        if (!catalogSuccess)
        {
            OnDownloadFailed?.Invoke("Failed to check for updates");
            yield break;
        }

        if (catalogsToUpdate != null && catalogsToUpdate.Count > 0)
        {
            OnStatusChanged?.Invoke("Updating catalogs...");
            OnDownloadProgress?.Invoke(0.3f);
            bool updateSuccess = false;
            var updateOp = Addressables.UpdateCatalogs(catalogsToUpdate, false);
            updateOp.Completed += handle => { updateSuccess = handle.Status == AsyncOperationStatus.Succeeded; };
            yield return updateOp;
            if (!updateSuccess)
            {
                OnDownloadFailed?.Invoke("Failed to update catalogs");
                yield break;
            }
        }

        OnStatusChanged?.Invoke("Checking download size...");
        OnDownloadProgress?.Invoke(0.4f);
        bool sizeSuccess = false;
        long downloadSize = 0;
        var sizeOp = Addressables.GetDownloadSizeAsync("music");
        sizeOp.Completed += handle =>
        {
            sizeSuccess = handle.Status == AsyncOperationStatus.Succeeded;
            if (sizeSuccess) downloadSize = handle.Result;
        };
        yield return sizeOp;
        if (!sizeSuccess)
        {
            OnDownloadFailed?.Invoke("Failed to check download size");
            yield break;
        }

        if (downloadSize > 0)
        {
            OnStatusChanged?.Invoke("Downloading music...");
            bool downloadSuccess = false;
            var downloadOp = Addressables.DownloadDependenciesAsync("music", false);
            downloadOp.Completed += handle => { downloadSuccess = handle.Status == AsyncOperationStatus.Succeeded; };
            while (!downloadOp.IsDone)
            {
                float progress = 0.4f + downloadOp.PercentComplete * 0.6f;
                OnDownloadProgress?.Invoke(progress);
                yield return null;
            }
            if (!downloadSuccess)
            {
                OnDownloadFailed?.Invoke("Failed to download music");
                yield break;
            }
        }
        else
        {
            OnDownloadProgress?.Invoke(0.9f);
        }

        OnDownloadProgress?.Invoke(1f);
        OnStatusChanged?.Invoke("Ready!");
        yield return new WaitForSeconds(0.3f);
        OnDownloadComplete?.Invoke();
#endif
    }
}
