using System.Collections;

public class BasicAttackAction : AIDecision<RangedAttackData>
{
    
    public override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        Brain.Piece.RotateTo(Brain.Player.Occupying);
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.WolfAttack);

        if (Data.Effect != null)
        {
            EffectSequence sequence = Instantiate(Data.Effect);
            yield return sequence.PlaySeqeunce(Brain.Player.transform.position);
        }
        
        Brain.Player.TakeDamage(Data.Damage);

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
