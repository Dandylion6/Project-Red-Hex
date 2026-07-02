using System.Collections;
using UnityEngine;


public abstract class AIDecision : MonoBehaviour
{
    public abstract ItemData BaseData { get; }
    public DecisionBrain Brain => brain;
    public int Cooldown => cooldown;

    private DecisionBrain brain = null;
    private int cooldown = 0;


    public virtual void Initialize(DecisionBrain brain) => this.brain = brain;

    public void StartCooldown() => cooldown = BaseData.Cooldown;
    public void UpdateCooldown() => cooldown = Mathf.Max(cooldown - 1, 0);

    public abstract bool IsValidAction();
    public abstract IEnumerator ActionSequence();
}


public abstract class AIDecision<T> : AIDecision where T : ItemData
{
    [Header("Action Settings")]
    [SerializeField] private T data = null;


    public override ItemData BaseData => data;

    protected T Data => data;
}