using System.Collections;
using UnityEngine;

public class BasicAttackAction : AIDecision<RangedAttackData>
{
    public override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        Brain.Piece.RotateTo(Brain.Player.Occupying);

        yield return TurnManager.TurnWait;

        Brain.Player.TakeDamage(Data.Damage);
        TurnManager.Instance.EndTurn();
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance > Data.AttackRange) return false;
        if (!Data.IgnoresObstacles)
        {
            if (!HexGridManager.Instance.InLineOfSight(Brain.Piece.Occupying, Brain.Player.Occupying)) return false;
        }
        return true;
    }
}
