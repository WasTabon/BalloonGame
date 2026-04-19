using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

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
    }

    public void FadeIn(float duration = 1f)
    {
    }

    public void FadeOut(float duration = 0.5f)
    {
    }

    public void SetPaused(bool paused)
    {
    }

    public void UpdateVolume()
    {
    }
}
