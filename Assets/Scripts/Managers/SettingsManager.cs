using UnityEngine;

public class SettingsData
{
    public float masterVolume = 0.5f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
}


public class SettingsManager : Singleton<SettingsManager>
{
    static public SettingsData Settings => settings;

    private static readonly SettingsData settings = new();


    public void SetMasterVolume(float volume) => settings.masterVolume = Mathf.Lerp(0.0f, 1.0f, volume);

    public void SetMusicVolume(float volume) => settings.musicVolume = Mathf.Lerp(0.0f, 1.0f, volume);

    public void SetSfxVolume(float volume) => settings.sfxVolume = Mathf.Lerp(0.0f, 1.0f, volume);


    private void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
}
