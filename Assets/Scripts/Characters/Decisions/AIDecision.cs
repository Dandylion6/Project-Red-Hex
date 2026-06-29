using System.Collections;
using UnityEngine;


public abstract class AIDecision : MonoBehaviour
{
    [Header("Action Settings")]
    [SerializeField] private int cooldown = 0;


    public DecisionBrain Brain => brain;

    private DecisionBrain brain = null;


    public virtual void Initialize(DecisionBrain brain) => this.brain = brain;

    public void StartCooldown() => cooldown = Mathf.Max(cooldown - 1, 0);

    public abstract bool IsValidAction();
    public abstract IEnumerator ActionSequence();
}
