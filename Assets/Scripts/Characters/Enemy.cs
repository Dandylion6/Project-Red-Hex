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
        if (!isInCombat) EnterCombatCheck();

        if (piece != this) return;
        if (!TurnManager.Instance.HasTurn(this)) return;
        StartCoroutine(TakeTurn());
    }

    
    protected virtual IEnumerator TakeTurn()
    {
        Debug.Log("Enemy taking turn.");
        yield return new WaitForSeconds(1.0f);
        TurnManager.Instance.EndTurn();
    }
}
