using System.Collections;
using UnityEngine;

public abstract class EffectSequence : MonoBehaviour
{
    public abstract IEnumerator PlaySeqeunce(Transform attachTo);
}
