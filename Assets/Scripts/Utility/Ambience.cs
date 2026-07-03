using UnityEngine;

public class Ambience : MonoBehaviour { 
    [SerializeField] private AudioClip ambienceList;

    private AudioSource source;
    
    public void Start()
    {
        source = AudioManager.Instance.PlayLoop(ambienceList, 1);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.StopLoop(source);
    }


}
