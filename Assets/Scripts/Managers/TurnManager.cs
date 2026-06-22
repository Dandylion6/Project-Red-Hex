using System.Collections.Generic;

public class TurnManager : Singleton<TurnManager>
{
    private readonly List<TilePiece> piecesWithTurns = new();
    private TilePiece activePiece = null;


    private void Start()
    {
        piecesWithTurns.Add(GameManager.Instance.Player);
    }


    private void Update()
    {
        if (activePiece != null && activePiece.HasTurn)
            activePiece = null;

        foreach (TilePiece piece in piecesWithTurns)
        {
            activePiece = piece;
            piece.StartTurn();
        }
    }
}
