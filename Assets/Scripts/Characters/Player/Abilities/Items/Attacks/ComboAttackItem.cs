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

                float rotation = 120.0f - Player.Rotation;
                bool lookingRight = Player.Rotation >= 30.0f && Player.Rotation <= 210.0f;
                sequence.transform.localScale = new(1.0f, lookingRight ? 1.0f : -1.0f, 1.0f);
                sequence.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);

                yield return sequence.PlaySeqeunce(Player.transform);
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
