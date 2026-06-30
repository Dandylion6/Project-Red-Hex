using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TilePiece))]
public class DecisionBrain : MonoBehaviour
{
    [Header("Brain Settings")]
    [SerializeField] private List<AIDecision> decisions = new();


    public TilePiece Player => player;
    public TilePiece Piece => piece;

    private TilePiece player = null;
    private TilePiece piece = null;


    private void Start()
    {
        player = GameManager.Instance.Player;
        piece = GetComponent<TilePiece>();
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);

        foreach(AIDecision decision in decisions)
            decision.Initialize(this);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        if (currentPiece != piece) return;

        foreach(AIDecision decision in decisions)
        {
            decision.UpdateCooldown();
            
            if (decision.Cooldown > 0) continue;
            if (!decision.IsValidAction()) continue;

            StartCoroutine(decision.ActionSequence());
            return;
        }

        // Can't do anything.
        TurnManager.Instance.EndTurn();
    }


    private void OnDestroy()
    {
        if (TurnManager.Instance == null) return;
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }
}
