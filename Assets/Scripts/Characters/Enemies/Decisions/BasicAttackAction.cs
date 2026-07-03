using System.Collections;
using UnityEngine;

public class BasicAttackAction : AIDecision<RangedAttackData>
{
    public override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        Brain.Piece.RotateTo(Brain.Player.Occupying);

        if (Data.Effect != null)
        {
            EffectSequence sequence = Instantiate(Data.Effect);
            Brain.Player.TakeDamage(Data.Damage, AudioManager.Instance.AudioBank.WolfAttack);
            yield return sequence.PlaySeqeunce(Brain.Piece);
        }

        

        yield return TurnManager.TurnWait;
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
