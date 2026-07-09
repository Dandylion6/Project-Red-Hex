using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup group = null;
    [SerializeField] private Slider masterSlider = null;
    [SerializeField] private Slider ambienceSlider = null;
    [SerializeField] private Slider sfxSlider = null;

    private bool isVisible = false;


    public void OnMasterVolumeSlider(float volume) => SettingsManager.Instance.SetMasterVolume(volume);

    public void OnAmbienceVolumeSlider(float volume) => SettingsManager.Instance.SetMusicVolume(volume);

    public void OnSfxVolumeSlider(float volume) => SettingsManager.Instance.SetSfxVolume(volume);


    public void OnSettingsToggle()
    {
        isVisible = !isVisible;
        group.interactable = isVisible;
        group.blocksRaycasts = isVisible;
        group.DOKill();
        group.DOFade(isVisible ? 1.0f : 0.0f, 0.25f).SetEase(Ease.InOutSine).SetUpdate(true).Play();
        Time.timeScale = isVisible ? 0.0f : 1.0f;
    }


    public void BackToMenu() => GameManager.Instance.GoBackToMenuAsync();


    private void Start()
    {
        masterSlider.value = SettingsManager.Settings.masterVolume;
        ambienceSlider.value = SettingsManager.Settings.musicVolume;
        sfxSlider.value = SettingsManager.Settings.sfxVolume;
        group.alpha = 0.0f;
        group.interactable = isVisible;
        group.blocksRaycasts = isVisible;
    }
}
