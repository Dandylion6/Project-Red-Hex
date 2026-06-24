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
    public bool IsInCombat => isInCombat;


    private readonly List<TilePiece> piecesWithTurns = new();

    private TilePiece pieceWithTurn = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private bool isInCombat = false;


    public void SetState(State state) => currentState = state;


    public void AddPieceToTurns(TilePiece piece)
    {
        if (!piecesWithTurns.Contains(piece)) piecesWithTurns.Add(piece);
        if (piecesWithTurns.Count > 1) isInCombat = true;
    }

    public bool IsPeiceWithTurn(TilePiece peice) => peice == pieceWithTurn;


    public void RemovePieceFromTurns(TilePiece piece)
    {
        if (piecesWithTurns.Contains(piece)) piecesWithTurns.Remove(piece);
        if (piecesWithTurns.Count <= 1) isInCombat = false;
    }


    private void Start()
    {
        piecesWithTurns.Add(GameManager.Instance.Player);
        AdvanceTurn();
    }


    private void Update()
    {
        if (pieceWithTurn != null && !pieceWithTurn.HasTurn)
            AdvanceTurn();
    }


    private void AdvanceTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % piecesWithTurns.Count;
        pieceWithTurn = piecesWithTurns[currentTurnIndex];
        pieceWithTurn.StartTurn();
    }
}
