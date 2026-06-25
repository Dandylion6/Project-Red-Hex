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


    private readonly List<TilePiece> piecesWithTurns = new();

    private TilePiece pieceWithTurn = null;
    private State currentState = State.None;
    private int currentTurnIndex = -1;
    private int turnsInCombat = 0;
    private bool isInCombat = false;
    private bool isInAction = false;


    public void SetState(State state) => currentState = state;

    public void StartAction() => isInAction = true;
    public void EndAction() => isInAction = false;


    public void AddPieceToTurns(TilePiece piece)
    {
        if (!piecesWithTurns.Contains(piece)) piecesWithTurns.Add(piece);
        if (piecesWithTurns.Count > 1)
        {
            isInCombat = true;
            turnsInCombat = 1; // Will begin counting at one.
        }
    }

    public bool IsPeiceWithTurn(TilePiece peice)
    {
        if (peice != pieceWithTurn) return false;
        if (isInAction) return false; // Can't 
        return true;
    }


    public void RemovePieceFromTurns(TilePiece piece)
    {
        if (piecesWithTurns.Contains(piece)) piecesWithTurns.Remove(piece);
        if (piecesWithTurns.Count <= 1)
        {
            isInCombat = false;
            turnsInCombat = 0;
            pieceWithTurn = GameManager.Instance.Player; // Player regains the turn after finishing combat.
        }
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

        if (isInCombat)
            ++turnsInCombat;
    }
}
