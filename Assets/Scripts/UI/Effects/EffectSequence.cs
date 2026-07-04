using System.Collections;
using UnityEngine;

public abstract class EffectSequence : MonoBehaviour
{
    public abstract IEnumerator PlaySeqeunce(Vector3 position);
}
