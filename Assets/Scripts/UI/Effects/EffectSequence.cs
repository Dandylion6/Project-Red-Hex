using System.Collections;
using UnityEngine;

public abstract class EffectSequence : MonoBehaviour
{
    public abstract IEnumerator PlaySeqeunce(TilePiece piece);
}
