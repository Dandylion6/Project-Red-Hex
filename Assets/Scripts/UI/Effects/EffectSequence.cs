using System.Collections;
using UnityEngine;

public struct EffectData
{
    public TilePiece piece;
    public TilePiece target;
}


public abstract class EffectSequence : MonoBehaviour
{
    public abstract IEnumerator PlaySeqeunce(EffectData data);
}
