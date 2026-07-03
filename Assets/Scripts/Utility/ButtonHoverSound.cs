using UnityEngine;

public class ButtonHoverSound : MonoBehaviour
{
    public void buttonSFX()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.AudioBank.ButtonHover, SettingsManager.Instance.GameVolume);
    }
}
