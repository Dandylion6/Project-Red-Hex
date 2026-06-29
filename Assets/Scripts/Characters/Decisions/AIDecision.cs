using System.Collections;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    [Header("Action Settings")]
    [SerializeField] private int cooldown = 0;


    public void StartCooldown() => cooldown = Mathf.Max(cooldown - 1, 0);

    public abstract bool IsValidAction();
    public abstract IEnumerator ActionSequence();
}
