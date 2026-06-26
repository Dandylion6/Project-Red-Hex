using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum State
    {
        None,
        UseItem,
        Move,
    }


    public TilePiece PieceWithTurn => pieceWithTurn;
    public State CurrentState => currentState;
    public int TurnsInCombat => turnsInCombat;
    public bool IsInCombat => isInCombat;


    private readonly List<TilePiece> activePieces = new();

    private Action<TilePiece, TilePiece> onTurnChanged = null;
    private TilePiece pieceWithTurn = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private int turnsInCombat = 0;
    private bool isInCombat = false;


    public void SubscribeToOnTurnChanged(Action<TilePiece, TilePiece> callback) => onTurnChanged += callback;
    public void UnsubscribeFromOnTurnChanged(Action<TilePiece, TilePiece> callback) => onTurnChanged -= callback;


    public void SetState(State state) => currentState = state;


    public void AddPieceToTurns(TilePiece piece)
    {
        if (!activePieces.Contains(piece)) activePieces.Add(piece);
    }


    public bool HasTurn(TilePiece peice)
    {
        if (peice != pieceWithTurn) return false;
        return true;
    }


    public void EndTurn()
    {
        if (IsStartOfCombat()) BeginCombat(); // Starting combat will give the player the first turn.
        else AdvanceTurn();
    }




    public void RemovePieceFromTurns(TilePiece piece)
    {
        if (activePieces.Contains(piece)) activePieces.Remove(piece);
        if (activePieces.Count <= 1)
        {
            isInCombat = false;
            turnsInCombat = 0;

            TilePiece lastPiece = pieceWithTurn;
            pieceWithTurn = GameManager.Instance.Player; // Player regains the turn after finishing combat.
            currentState = State.None;
            
            currentTurnIndex = 0;
            onTurnChanged?.Invoke(pieceWithTurn, lastPiece);
        }
    }


    private void Start()
    {
        activePieces.Add(GameManager.Instance.Player);
        pieceWithTurn = GameManager.Instance.Player;
    }


    private bool IsStartOfCombat()
    {
        if (isInCombat) return false;
        if (turnsInCombat > 1) return false;
        return true;
    }


    private void BeginCombat()
    {
        isInCombat = true;
        currentTurnIndex = 0; // Let the player act first.
        turnsInCombat = 1;

        TilePiece lastPiece = pieceWithTurn;
        pieceWithTurn = GameManager.Instance.Player; // Player regains the turn after finishing combat.
        currentState = State.None;

        onTurnChanged?.Invoke(pieceWithTurn, lastPiece);
    }


    private void AdvanceTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % activePieces.Count;

        TilePiece lastPiece = pieceWithTurn;
        pieceWithTurn = activePieces[currentTurnIndex];

        if (isInCombat)
            ++turnsInCombat;

        onTurnChanged?.Invoke(pieceWithTurn, lastPiece);
    }
}
