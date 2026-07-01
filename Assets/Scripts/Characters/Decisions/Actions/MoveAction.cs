using System.Collections;
using System.Collections.Generic;

public class MoveAction : AIDecision<ItemData>
{
    public override IEnumerator ActionSequence()
    {
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(Brain.Player.Occupying, Brain.Piece.Occupying);

        if (result.path.Count == 0)
        {
            TurnManager.Instance.EndTurn();
            yield break;
        }
        Stack<HexTile> path = result.path;

        while (path.Count > 0)
        {
            HexTile tile = path.Pop();
            int distance = Hexagon.HexDistance(Brain.Piece.Occupying, tile);
            if (distance > Brain.Piece.MaxMoveDistance) continue;

            TurnManager.Instance.StartAction();
            StartCooldown();
            yield return TurnManager.TurnWait;

            Brain.Piece.RotateTo(tile);
            if (!Brain.Piece.MoveTo(tile))
            {
                // Failed to move.
                TurnManager.Instance.EndTurn();
                break;
            }
              
        }
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance <= 1) return false; // Can't move anymore.
        return true;
    }
}
