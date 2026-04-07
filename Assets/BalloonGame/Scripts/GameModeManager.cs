using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameMode CurrentMode { get; private set; } = GameMode.Classic;

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

    public void SetMode(GameMode mode)
    {
        CurrentMode = mode;
    }

    public string GetModeName()
    {
        switch (CurrentMode)
        {
            case GameMode.Classic: return "CLASSIC";
            case GameMode.TimeAttack: return "TIME ATTACK";
            case GameMode.ShieldRush: return "SHIELD RUSH";
            default: return "CLASSIC";
        }
    }
}
