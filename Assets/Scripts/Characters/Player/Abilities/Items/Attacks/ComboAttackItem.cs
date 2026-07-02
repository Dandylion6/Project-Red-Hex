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
            if (!CanHit(combo)) break;

            yield return TurnManager.TurnWait;
            Hit(combo);
        }

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }


    private bool CanHit(ComboData combo)
    {
        if (Target == null) return false;

        float chance = Random.Range(0.0f, 100.0f);
        if (chance > combo.hitChance) return false;
        return true;
    }


    private void Hit(ComboData combo)
    {
        if (Target == null) return;
        int damage = Mathf.RoundToInt(Data.Damage * combo.damageMultiplier);
        Target.TakeDamage(damage);
    }
}
