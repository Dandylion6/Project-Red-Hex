using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    private float musicVolume = 50f;

    private float gameVolume = 50f;

    private bool activeVibrations;

    public void setMusicVolume(float sliderInt)
    {
        musicVolume = Mathf.Clamp(sliderInt, 0, 100);
        Debug.Log(musicVolume);
    }

    public void setGameVolume(float sliderInt)
    {
        gameVolume = Mathf.Clamp(sliderInt, 0, 100);
        Debug.Log(gameVolume);
    }


}
