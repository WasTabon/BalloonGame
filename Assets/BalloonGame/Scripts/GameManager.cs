using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

        BestScore = GetBestScore(GameMode.Classic);
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

    public int GetBestScore(GameMode mode)
    {
        string key = "BestScore_" + mode.ToString();
        return PlayerPrefs.GetInt(key, 0);
    }

    public bool TrySetBestScore(int score)
    {
        GameMode mode = GameModeManager.Instance != null ? GameModeManager.Instance.CurrentMode : GameMode.Classic;
        return TrySetBestScore(score, mode);
    }

    public bool TrySetBestScore(int score, GameMode mode)
    {
        int current = GetBestScore(mode);
        if (score > current)
        {
            string key = "BestScore_" + mode.ToString();
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();

            if (mode == GameMode.Classic)
            {
                BestScore = score;
                OnBestScoreChanged?.Invoke(BestScore);
            }
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
