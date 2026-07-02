using UnityEngine;

public class EffectsUI : Singleton<EffectsUI>
{
    private Camera mainCamera = null;


    public void AddEffect(EffectSequence sequence, Vector3 position)
    {
        sequence.transform.SetParent(transform);
        sequence.transform.position = mainCamera.WorldToScreenPoint(position);
    }


    private void Start() => mainCamera = Camera.main;
}
