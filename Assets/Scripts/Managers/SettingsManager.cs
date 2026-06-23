using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{

    private static SettingsData settingsData;

    [SerializeField] private Scrollbar gameVolume;

    [SerializeField] private Scrollbar musicVolume;


    public void setMusicVolume(Scrollbar musicVolume)
    {
        settingsData.musicVolume = Mathf.Clamp(gameVolume.value, 0, 100);
        Debug.Log(settingsData.musicVolume);
    }

    public void setGameVolume(Scrollbar gameVolume)
    {
        settingsData.gameVolume = Mathf.Clamp(gameVolume.value, 0, 100);
        Debug.Log(settingsData.gameVolume);
    }


}
