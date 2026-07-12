using System.Collections;
using UnityEngine;

public struct EffectData
{
    public TilePiece piece;
    public TilePiece target;
    public Vector3 offset;
}


public abstract class EffectSequence : MonoBehaviour
{
    public abstract IEnumerator PlaySeqeunce(EffectData data);


    private void Start()
    {
        if (!TryGetComponent(out Canvas canvas)) return;
        canvas.worldCamera = GameManager.Instance.EffectsCamera;
    }
}
