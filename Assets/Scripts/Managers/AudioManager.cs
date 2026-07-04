using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public class SourceInfo
    {
        public AudioSource source;
        public Transform attachedTransform;
        public Vector3 offset;
        public float volume;
        public bool isAttached;
    }


    [Header("References")]
    [SerializeField] AudioBank audioBank = null;


    public AudioBank AudioBank => audioBank;

    private readonly Queue<SourceInfo> audioPool = new();
    private readonly List<SourceInfo> activeSources = new();
    private float globalVolume = 1.0f;


    public void SetGlobalVolumeTo(float volume)
    {
        globalVolume = volume;
    }
    

    public SourceInfo AddSound(AudioClip clip, float volume = 1.0f)
    {
        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0.0f;
        source.loop = true;

        sourceInfo.volume = volume;
        sourceInfo.isAttached = false;
        activeSources.Add(sourceInfo);
        return sourceInfo;
    }


    public SourceInfo AddSound3D(AudioClip clip, Transform attachTo, Vector3 offset = new(), float volume = 1.0f)
    {
        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1.0f;
        source.loop = true;

        sourceInfo.volume = volume;
        sourceInfo.attachedTransform = attachTo;
        sourceInfo.isAttached = true;

        sourceInfo.offset = offset;
        activeSources.Add(sourceInfo);
        return sourceInfo;
    }


    public void PlayOneShot(AudioClip clip, float volume = 1.0f, bool is3D = false, Vector3 position = new())
    {
        if (clip == null) return;

        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        sourceInfo.isAttached = false;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = is3D ?  1.0f : 0.0f;
        transform.position = position;
        source.loop = false;

        source.Stop();
        source.time = 0.0f;
        source.Play();

        sourceInfo.volume = volume;
        activeSources.Add(sourceInfo);
    }


    public void PlayOneShotRandom(AudioClip[] clips, float volume = 1.0f, bool is3D = false, Vector3 position = new())
    {
        bool hasClips = clips.Length > 0;
        if (!hasClips) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        PlayOneShot(clip, volume, is3D, position);
    }


    public void PlayOneShot(AudioClip clip, Transform attachTo, Vector3 offset = new(), float volume = 1.0f)
    {
        if (clip == null) return;

        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1.0f;
        source.loop = false;

        source.Stop();
        source.time = 0.0f;
        source.Play();

        sourceInfo.volume = volume;
        sourceInfo.attachedTransform = attachTo;
        sourceInfo.isAttached = true;
        sourceInfo.offset = offset;
        activeSources.Add(sourceInfo);
    }


    public void PlayOneShotRandom(AudioClip[] clips, Transform attachTo, Vector3 offset = new(), float volume = 1.0f)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        PlayOneShot(clip, attachTo, offset, volume);
    }


    public AudioSource PlayLoop(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null) return null;

        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0.0f;
        source.loop = true;
        sourceInfo.isAttached = false;

        source.Play();

        sourceInfo.volume = volume;
        activeSources.Add(sourceInfo);
        return source;
    }


    public AudioSource PlayLoop(AudioClip clip, Transform attachTo, Vector3 offset = new(), float volume = 1f)
    {
        if (clip == null) return null;

        SourceInfo sourceInfo = GetSource();
        AudioSource source = sourceInfo.source;
        source.clip = clip;
        source.spatialBlend = 1.0f;
        source.loop = true;

        source.Play();

        sourceInfo.volume = volume;
        sourceInfo.attachedTransform = attachTo;
        sourceInfo.isAttached = true;
        sourceInfo.offset = offset;
        activeSources.Add(sourceInfo);
        return source;
    }


    /// <summary>
    /// Stops playback on the specified audio source and returns it to the audio pool for reuse.
    /// </summary>
    /// <remark>
    /// Once a source has stopped is gtets put into the pool, make sure it isn't referenced after.
    /// </remark>
    /// <remarks>If the specified source is not currently active, this method has no effect.</remarks>
    /// <param name="source">The audio source to stop and return to the pool. Cannot be null.</param>
    public void StopLoop(AudioSource source)
    {
        source.Stop();
        foreach (SourceInfo sourceInfo in activeSources)
        {
            bool foundSource = sourceInfo.source == source;
            if (foundSource)
            {
                activeSources.Remove(sourceInfo);
                audioPool.Enqueue(sourceInfo);
                break;
            }
        }
    }


    private void Start()
    {
        globalVolume = 1.0f;
    }


    private void Update()
    {
        List<SourceInfo> toClean = new();
        foreach (SourceInfo sourceInfo in activeSources)
        {
            UpdatePositon(sourceInfo);
            UpdateActive(sourceInfo, toClean);
        }

        bool hasCleanup = toClean.Count > 0;
        if (!hasCleanup) return;

        foreach (SourceInfo sourceInfo in toClean)
            activeSources.Remove(sourceInfo);
    }


    private void UpdateActive(SourceInfo sourceInfo, List<SourceInfo> toClean)
    {
        bool sourceExists = sourceInfo.source != null;
        if (!sourceExists)
        {
            toClean.Add(sourceInfo);
            return;
        }

        AudioSource source = sourceInfo.source;
        bool endSource = !source.isPlaying && !source.loop;
        if (endSource)
        {
            source.gameObject.SetActive(false);
            audioPool.Enqueue(sourceInfo);
            toClean.Add(sourceInfo);

            sourceInfo.attachedTransform = null;
            sourceInfo.offset = Vector3.zero;
            return;
        }

        bool paused = Time.timeScale <= 0.0f;
        if (paused)
        {
            if (source.isPlaying) source.Pause();
        }
        else if (!source.isPlaying)
        {
            source.pitch = Time.timeScale;
            source.UnPause();
        }

        source.volume = sourceInfo.volume * globalVolume;
        bool fadeOut = sourceInfo.isAttached && sourceInfo.attachedTransform == null;
        if (fadeOut)
        {
            sourceInfo.isAttached = false;
            // TODO: Add DOTween
            //source.DOFade(0.0f, 0.8f).SetEase(Ease.InOutSine).Play();
            // Will be later cleaned up.
        }
    }


    private void UpdatePositon(SourceInfo sourceInfo)
    {
        bool transformExists = sourceInfo.attachedTransform != null;
        if (!transformExists) return;

        Vector3 newPosition = sourceInfo.attachedTransform.position + sourceInfo.offset;
        sourceInfo.source.transform.position = newPosition;
    }


    
    
    private SourceInfo GetSource()
    {
        while (audioPool.Count > 0)
        {
            SourceInfo pooled = audioPool.Dequeue();
            if (pooled == null || pooled.source == null) continue; // stale, discard
            pooled.source.gameObject.SetActive(true);
            return pooled;
        }

        GameObject gameObject = new("Sound");
        gameObject.transform.SetParent(transform);
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.minDistance = 4.0f;
        return new SourceInfo { source = source };
    }
}
