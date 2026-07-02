using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : AIDecision<ItemData>
{
    public override IEnumerator ActionSequence()
    {
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(Brain.Piece.Occupying, Brain.Player.Occupying);
        List<HexTile> path = result.path;

        if (path.Count <= 1)
        {
            TurnManager.Instance.EndTurn();
            yield break;
        }

        int i = Mathf.Min(path.Count - 1, Brain.Piece.MaxMoveDistance) - 1;
        HexTile tile = path[i];

        TurnManager.Instance.StartAction();
        StartCooldown();
        yield return TurnManager.TurnWait;

        Brain.Piece.RotateTo(tile);
        if (!Brain.Piece.MoveTo(tile))
            yield break;

        TurnManager.Instance.EndTurn();
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance <= 1) return false; // Can't move anymore.
        return true;
    }
}
