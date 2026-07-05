using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    private class EffectData
    {
        public readonly EffectSequence effect = null;
        public readonly Transform attachTo = null;
        public readonly Vector3 offset = Vector3.zero;

        public EffectData(EffectSequence effect, Transform attachTo, Vector3 offset)
        {
            this.effect = effect;
            this.attachTo = attachTo;
            this.offset = offset;
        }
    }


    private readonly List<EffectData> effects = new();
    private Camera mainCamera = null;


    public void AddEffect(EffectSequence sequence, Transform attachTo, Vector3 offset)
    {
        sequence.transform.SetParent(transform);
        effects.Add(new(sequence, attachTo, offset));
    }


    private void Start() => mainCamera = Camera.main;


    private void Update()
    {
        for (int i = 0; i < effects.Count; ++i)
        {
            EffectData data = effects[i];
            if (ShouldRemove(data))
            {
                effects.RemoveAt(i);
                --i;
                continue;
            }

            EffectSequence sequence = data.effect;
            Vector3 position = data.attachTo.position + data.offset;
            sequence.transform.position = mainCamera.WorldToScreenPoint(position);
        }
    }


    private bool ShouldRemove(EffectData data)
    {
        if (data.effect == null) return true;
        if (data.attachTo == null) return true;
        return false;
    }
}
