using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private bool musicLoaded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    public void LoadAndPlay()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return;
#else
        if (musicLoaded) return;
        Addressables.LoadAssetAsync<AudioClip>("GameMusic").Completed += OnMusicLoaded;
#endif
    }

    private void OnMusicLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            musicLoaded = true;
            audioSource.clip = handle.Result;
            audioSource.Play();
            FadeIn();
        }
        else
        {
            Debug.LogWarning("MusicManager: Failed to load music");
        }
    }

    public void FadeIn(float duration = 1f)
    {
        float targetVol = GameManager.Instance != null && GameManager.Instance.SoundEnabled ? 0.4f : 0f;
        audioSource.DOKill();
        audioSource.DOFade(targetVol, duration).SetUpdate(true);
    }

    public void FadeOut(float duration = 0.5f)
    {
        audioSource.DOKill();
        audioSource.DOFade(0f, duration).SetUpdate(true);
    }

    public void SetPaused(bool paused)
    {
        if (paused)
            audioSource.Pause();
        else
            audioSource.UnPause();
    }

    public void UpdateVolume()
    {
        if (GameManager.Instance == null) return;
        float targetVol = GameManager.Instance.SoundEnabled ? 0.4f : 0f;
        audioSource.DOKill();
        audioSource.DOFade(targetVol, 0.3f).SetUpdate(true);
    }
}
