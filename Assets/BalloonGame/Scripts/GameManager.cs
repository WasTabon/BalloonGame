using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string BEST_SCORE_KEY = "BestScore";
    private const string SOUND_KEY = "SoundEnabled";

    public int BestScore { get; private set; }
    public bool SoundEnabled { get; private set; }

    public System.Action<int> OnBestScoreChanged;
    public System.Action<bool> OnSoundChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        SoundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        AudioListener.volume = SoundEnabled ? 1f : 0f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    public bool TrySetBestScore(int score)
    {
        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, BestScore);
            PlayerPrefs.Save();
            OnBestScoreChanged?.Invoke(BestScore);
            return true;
        }
        return false;
    }

    public void SetSoundEnabled(bool enabled)
    {
        SoundEnabled = enabled;
        PlayerPrefs.SetInt(SOUND_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
        AudioListener.volume = enabled ? 1f : 0f;
        OnSoundChanged?.Invoke(enabled);
    }

    public void ToggleSound()
    {
        SetSoundEnabled(!SoundEnabled);
    }
}
