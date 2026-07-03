using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{

    private static SettingsData settingsData;

    public float MusicVolume => settingsData.musicVolume;

    public float GameVolume => settingsData.gameVolume;

    public void setMusicVolume(Slider musicVolume)
    {
        settingsData.musicVolume = Mathf.Clamp(musicVolume.value * 100, 0, 100);
        AudioManager.Instance.setGlobalVolumeTo(musicVolume.value);
        Debug.Log(settingsData.musicVolume);
    }

    public void setGameVolume(Slider gameVolume)
    {
        settingsData.gameVolume = Mathf.Clamp(gameVolume.value * 100, 0, 100);
        Debug.Log(settingsData.gameVolume);
    }

    public void PlayUIClip()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.AudioBank.UIClick, SettingsManager.Instance.GameVolume, false);
    }


}
