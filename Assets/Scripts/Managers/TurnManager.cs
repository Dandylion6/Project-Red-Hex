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


    public State CurrentState => currentState;
    public bool IsInCombat => isInCombat;


    private readonly List<TilePiece> piecesWithTurns = new();

    private TilePiece activePiece = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private bool isInCombat = false;


    public void SetState(State state) => currentState = state;


    private void Start()
    {
        piecesWithTurns.Add(GameManager.Instance.Player);
        AdvanceTurn();
    }


    private void Update()
    {
        if (activePiece != null && !activePiece.HasTurn)
            AdvanceTurn();
    }


    private void AdvanceTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % piecesWithTurns.Count;
        activePiece = piecesWithTurns[currentTurnIndex];
        activePiece.StartTurn();
    }
}
