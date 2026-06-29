using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : AIDecision
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

            Brain.Piece.MoveTo(tile);
            break;
        }
        yield return null;
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        Debug.Log(distance);
        if (distance <= 1) return false; // Can't move anymore.
        return true;
    }
}
