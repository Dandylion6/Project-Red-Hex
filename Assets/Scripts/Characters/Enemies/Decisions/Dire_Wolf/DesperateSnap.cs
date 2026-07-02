using System.Collections;
using System.Collections.Generic;

public class DesperateSnap : AIDecision<DesperationAttackAction>
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

        int i = path.Count - 2; // Will always get to the player.
        HexTile tile = path[i];

        TurnManager.Instance.StartAction();
        StartCooldown();
        yield return TurnManager.TurnWait;

        Brain.Piece.RotateTo(tile);
        if (Brain.Piece.MoveTo(tile, true))
        {
            yield return TurnManager.TurnWait;
            Brain.Player.TakeDamage(Data.Damage);
        }
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance > Data.AttackRange) return false;

        float healthPercentage = (Brain.Piece.Health / (float)Brain.Piece.MaxHealth) * 100.0f;
        if (healthPercentage > Data.DesperationThreshold) return false;
        return true;
    }
}
