using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance { get; private set; }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _hapticLight();
    [DllImport("__Internal")]
    private static extern void _hapticMedium();
    [DllImport("__Internal")]
    private static extern void _hapticHeavy();
#endif

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

    public void Light()
    {
        if (GameManager.Instance != null && !GameManager.Instance.SoundEnabled) return;
#if UNITY_IOS && !UNITY_EDITOR
        _hapticLight();
#else
        Handheld.Vibrate();
#endif
    }

    public void Medium()
    {
        if (GameManager.Instance != null && !GameManager.Instance.SoundEnabled) return;
#if UNITY_IOS && !UNITY_EDITOR
        _hapticMedium();
#else
        Handheld.Vibrate();
#endif
    }

    public void Heavy()
    {
        if (GameManager.Instance != null && !GameManager.Instance.SoundEnabled) return;
#if UNITY_IOS && !UNITY_EDITOR
        _hapticHeavy();
#endif
    }
}
