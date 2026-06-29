using NUnit.Framework.Internal.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Hardware;
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
    private Action<TilePiece> onPieceAdded = null;
    private Action<TilePiece> onPieceRemoved = null;
    private TilePiece pieceWithTurn = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private int turnsInCombat = 0;
    private bool isInCombat = false;


    public void SubscribeToOnTurnChanged(Action<TilePiece, TilePiece> callback) => onTurnChanged += callback;
    public void UnsubscribeFromOnTurnChanged(Action<TilePiece, TilePiece> callback) => onTurnChanged -= callback;

    public void SubscribeToOnPieceAdded(Action<TilePiece> callback) => onPieceAdded += callback;
    public void UnsubscribeFromOnPieceAdded(Action<TilePiece> callback) => onPieceAdded -= callback;

    public void SubscribeToOnPieceRemoved(Action<TilePiece> callback) => onPieceRemoved += callback;
    public void UnsubscribeFromOnPieceRemoved(Action<TilePiece> callback) => onPieceRemoved -= callback;


    public void SetState(State state) => currentState = state;


    public void AddPieceToTurns(TilePiece piece)
    {
        if (activePieces.Contains(piece)) return;

        activePieces.Add(piece);
        onPieceAdded?.Invoke(piece);
    }


    public bool HasTurn(TilePiece peice)
    {
        if (peice != pieceWithTurn) return false;
        return true;
    }


    public void StartAction()
    {
        TilePiece lastPiece = pieceWithTurn;
        pieceWithTurn = null; // No one is allowed to act until end of turn.
        onTurnChanged?.Invoke(null, lastPiece);
    }


    public void EndTurn()
    {
        if (IsStartOfCombat()) BeginCombat(); // Starting combat will give the player the first turn.)
        else AdvanceTurn();
    }



    public void RemovePieceFromTurns(TilePiece piece)
    {
        if (!activePieces.Contains(piece)) return;

        activePieces.Remove(piece);
        onPieceRemoved?.Invoke(piece);

        if (activePieces.Count <= 1)
        {
            GivePlayerTurn();
            isInCombat = false;
            turnsInCombat = 0;
        }
    }


    private void GivePlayerTurn()
    {
        TilePiece lastPiece = pieceWithTurn;
        pieceWithTurn = GameManager.Instance.Player; // Player regains the turn after finishing combat.
        currentState = State.None;

        currentTurnIndex = 0;
        onTurnChanged?.Invoke(pieceWithTurn, lastPiece);
    }


    private IEnumerator Start()
    {
        activePieces.Add(GameManager.Instance.Player);
        yield return null;

        GivePlayerTurn();
    }


    private bool IsStartOfCombat()
    {
        if (activePieces.Count <= 1) return false;
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

        bool combatCycleDone = isInCombat && currentTurnIndex == 0;
        if (combatCycleDone) ++turnsInCombat;

        onTurnChanged?.Invoke(pieceWithTurn, lastPiece);
    }
}
