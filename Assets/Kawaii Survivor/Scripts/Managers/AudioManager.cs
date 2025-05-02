using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public bool IsSFXOn { get; private set; }
    public bool IsMusicOn { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance to this object
        }
        else
        {
            Destroy(gameObject); // Destroy this object if another instance already exists
        }
    }

    void OnEnable()
    {
        SettingsManager.onSFXStateChanged += SFXStateChangedCallback;
        SettingsManager.onMusicStateChanged += MusicStateChangedCallback;
    }

    void OnDisable()
    {
        SettingsManager.onSFXStateChanged -= SFXStateChangedCallback;
        SettingsManager.onMusicStateChanged -= MusicStateChangedCallback;
    }

    void OnDestroy()
    {
        SettingsManager.onSFXStateChanged -= SFXStateChangedCallback;
        SettingsManager.onMusicStateChanged -= MusicStateChangedCallback;
    }

    private void SFXStateChangedCallback(bool state) => IsSFXOn = state;
    private void MusicStateChangedCallback(bool state) => IsMusicOn = state;
}
