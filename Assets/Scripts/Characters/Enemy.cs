using System.Collections;
using UnityEngine;

public class Enemy : TilePiece
{
    [Header("Enemy Settings")]
    [SerializeField] private int combatRange = 4;


    protected TilePiece Player => player;

    private TilePiece player = null;
    private bool isInCombat = false;


    public override void Die()
    {
        base.Die();
        TurnManager.Instance.RemovePieceFromTurns(this);
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }


    private void Start()
    {
        player = GameManager.Instance.Player;
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }


    private void EnterCombatCheck()
    {
        int hexDistance = Hexagon.HexDistance(player.Occupying, Occupying);
        if (hexDistance > combatRange) return;

        TurnManager.Instance.AddPieceToTurns(this);
        isInCombat = true;
    }


    protected virtual void OnTurnChanged(TilePiece piece, TilePiece lastPiece)
    {
        if (!isInCombat) 
            EnterCombatCheck();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (IsDead) return;

        if (TurnManager.Instance == null) return;
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }
}
