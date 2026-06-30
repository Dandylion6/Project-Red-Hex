using System.Collections;
using System.Collections.Generic;

public class MoveAction : AIDecision<ItemData>
{
    public override IEnumerator ActionSequence()
    {
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(Brain.Player.Occupying, Brain.Piece.Occupying);
        Stack<HexTile> path = result.path;

        while (path.Count > 0)
        {
            HexTile tile = path.Pop();
            int distance = Hexagon.HexDistance(Brain.Piece.Occupying, tile);
            if (distance > Brain.Piece.MaxMoveDistance) continue;

            StartCooldown();
            Brain.Piece.MoveTo(tile);
            break;
        }
        yield return null;
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance <= 1) return false; // Can't move anymore.
        return true;
    }
}
