using DG.Tweening;
using UnityEngine;

public class Ambience : MonoBehaviour 
{
    [Header("Ambience Settings")]
    [SerializeField] private AudioClip ambienceTrack = null;
    [SerializeField] private float volume = 1.0f;

    private AudioSource source = null;
    
    public void Start()
    {
        source = AudioManager.Instance.PlayLoop(ambienceTrack, volume);
        if (source == null)
        {
            Destroy(gameObject);
            return;
        }
        source.volume = 0.0f; 
        
        // Will fade into the sound.
        source.DOKill();
        source.DOFade(volume, 1.5f).SetEase(Ease.InOutSine).Play();
    }


    private void OnDestroy()
    {
        if (AudioManager.Instance == null) return;
        if (source == null) return;
        AudioManager.Instance.StopLoop(source);
    }
}
