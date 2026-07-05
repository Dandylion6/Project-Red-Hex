using System.Collections;
using UnityEngine;

public class BasicAttackAction : AIDecision<RangedAttackData>
{
    [SerializeField][Tooltip("If true, the effect will originate from the piece toward the target. Otherwise it will appear on the player.")] private bool effectFromPiece = false;


    public override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        Brain.Piece.RotateTo(Brain.Player.Occupying);

        if (Data.Effect != null)
        {
            EffectSequence sequence = Instantiate(Data.Effect);
            EffectData data = new()
            {
                piece = Brain.Piece,
                target = Brain.Player,
            };
            yield return sequence.PlaySeqeunce(data);
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
