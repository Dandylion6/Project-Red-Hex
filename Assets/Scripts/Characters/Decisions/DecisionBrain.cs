using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TilePiece))]
public class DecisionBrain : MonoBehaviour
{
    [Header("Brain Settings")]
    [SerializeField] private List<AIDecision> decisions = new();


    private TilePiece piece = null;


    private void Start()
    {
        piece = GetComponent<TilePiece>();
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        if (currentPiece != piece) return;

        foreach(AIDecision decision in decisions)
        {
            if (!decision.IsValidAction()) continue;
            StartCoroutine(decision.ActionSequence());
            break;
        }
    }


    private void OnDestroy() => TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
}
