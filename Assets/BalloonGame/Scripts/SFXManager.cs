using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

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
        audioSource.playOnAwake = false;

        GenerateClips();
    }

    private void GenerateClips()
    {
        clips["shield_hit"] = CreateClip("shield_hit", 0.1f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            float freq = Mathf.Lerp(800f, 400f, t);
            float amp = 1f - t;
            return Mathf.Sin(2f * Mathf.PI * freq * t * 0.1f) * amp * 0.5f;
        });

        clips["balloon_pop"] = CreateClip("balloon_pop", 0.25f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            float noise = (Random.value * 2f - 1f) * (1f - t);
            float low = Mathf.Sin(2f * Mathf.PI * 80f * t * 0.25f) * (1f - t);
            return (noise * 0.4f + low * 0.6f) * 0.6f;
        });

        clips["score_tick"] = CreateClip("score_tick", 0.05f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            return Mathf.Sin(2f * Mathf.PI * 1200f * t * 0.05f) * (1f - t) * 0.15f;
        });

        clips["milestone"] = CreateClip("milestone", 0.3f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            float freq = Mathf.Lerp(600f, 1200f, t);
            float amp = t < 0.1f ? t / 0.1f : (1f - t);
            return Mathf.Sin(2f * Mathf.PI * freq * t * 0.3f) * amp * 0.4f;
        });

        clips["button_click"] = CreateClip("button_click", 0.06f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            return Mathf.Sin(2f * Mathf.PI * 600f * t * 0.06f) * (1f - t) * 0.25f;
        });

        clips["game_over"] = CreateClip("game_over", 0.5f, 44100, (i, len) =>
        {
            float t = (float)i / len;
            float freq = Mathf.Lerp(400f, 100f, t);
            float amp = 1f - t * 0.8f;
            return Mathf.Sin(2f * Mathf.PI * freq * t * 0.5f) * amp * 0.5f;
        });
    }

    private delegate float SampleFunc(int index, int totalSamples);

    private AudioClip CreateClip(string name, float duration, int sampleRate, SampleFunc sampleFunc)
    {
        int sampleCount = (int)(duration * sampleRate);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
            samples[i] = sampleFunc(i, sampleCount);
        var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public void Play(string clipName, float volumeScale = 1f)
    {
        if (GameManager.Instance != null && !GameManager.Instance.SoundEnabled) return;

        if (clips.TryGetValue(clipName, out var clip))
            audioSource.PlayOneShot(clip, volumeScale);
        else
            Debug.LogWarning($"SFXManager: clip '{clipName}' not found!");
    }

    public void PlayShieldHit() => Play("shield_hit");
    public void PlayBalloonPop() => Play("balloon_pop");
    public void PlayScoreTick() => Play("score_tick", 0.5f);
    public void PlayMilestone() => Play("milestone");
    public void PlayButtonClick() => Play("button_click");
    public void PlayGameOver() => Play("game_over");
}
