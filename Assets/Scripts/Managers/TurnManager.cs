using System;
using System.Collections.Generic;

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
    public bool IsInAction => isInAction;


    private readonly List<TilePiece> activePieces = new();

    private Action<TilePiece> onTurnChanged = null;
    private TilePiece pieceWithTurn = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private int turnsInCombat = 0;
    private bool isInCombat = false;
    private bool isInAction = false;


    public void SubscribeToOnTurnChanged(Action<TilePiece> callback) => onTurnChanged += callback;
    public void UnsubscribeFromOnTurnChanged(Action<TilePiece> callback) => onTurnChanged -= callback;


    public void SetState(State state) => currentState = state;

    public void StartAction() => isInAction = true;
    public void EndAction() => isInAction = false;


    public void AddPieceToTurns(TilePiece piece)
    {
        if (!activePieces.Contains(piece)) activePieces.Add(piece);
        if (activePieces.Count > 1)
        {
            isInCombat = true;
            turnsInCombat = 1; // Will begin counting at one.
        }
    }

    public bool HasTurn(TilePiece peice)
    {
        if (peice != pieceWithTurn) return false;
        if (isInAction) return false; // Can't 
        return true;
    }


    public void EndTurn()
    {
        isInAction = false;
        AdvanceTurn();
    }


    public void RemovePieceFromTurns(TilePiece piece)
    {
        if (activePieces.Contains(piece)) activePieces.Remove(piece);
        if (activePieces.Count <= 1)
        {
            isInCombat = false;
            isInAction = false;

            turnsInCombat = 0;
            pieceWithTurn = GameManager.Instance.Player; // Player regains the turn after finishing combat.
            currentState = State.None;
        }
    }


    private void Start()
    {
        activePieces.Add(GameManager.Instance.Player);
        AdvanceTurn();
    }


    private void AdvanceTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % activePieces.Count;
        pieceWithTurn = activePieces[currentTurnIndex];

        if (isInCombat)
            ++turnsInCombat;

        onTurnChanged?.Invoke(pieceWithTurn);
    }
}
