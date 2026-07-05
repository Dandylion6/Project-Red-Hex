using System.Collections;
using UnityEngine;

public class ComboAttackItem : RangedAttackItem<ComboAttackData>
{
    protected override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        foreach (ComboData combo in Data.Combo)
        {
            if (!CanHit(combo))
            {
                yield return TurnManager.TurnWait;
                TurnManager.Instance.EndTurn();
                yield break;
            }

            if (Data.Effect != null)
            {
                EffectSequence sequence = Instantiate(Data.Effect);
                EffectData data = new()
                {
                    piece = Player,
                    target = Target as TilePiece,
                };
                yield return sequence.PlaySeqeunce(data);
            }

            int damage = Mathf.RoundToInt(Data.Damage * combo.damageMultiplier);
            Target.TakeDamage(damage);

            if (!TargetIsValid())
            {
                yield return TurnManager.TurnWait;
                TurnManager.Instance.EndTurn();
                yield break;
            }

            yield return new WaitForSeconds(combo.comboDelay);
        }

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }


    private bool CanHit(ComboData combo)
    {
        if (!TargetIsValid()) return false;

        float chance = Random.Range(0.0f, 100.0f);
        if (chance > combo.hitChance) return false;
        return true;
    }


    private bool TargetIsValid()
    {
        if (Target == null) return false;

        TilePiece piece = Target as TilePiece;
        if (piece != null && piece.IsDead) return false;
        return true;
    }
}
