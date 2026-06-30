using System.Collections;
using UnityEngine;

public class ComboAttackItem : RangedAttackItem<ComboAttackData>
{
    protected override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        for (int i = 0; i < Data.MaxHits; ++i)
        {
            yield return new WaitForSeconds(0.5f);

            if (i < Data.MinHits)
            {
                Hit();
                continue;
            }

            if (!CanHit()) break;

            Hit();
        }

        TurnManager.Instance.EndTurn();
    }


    private bool CanHit()
    {
        float chance = Random.Range(0.0f, 100.0f);
        if (chance > Data.HitChance) return false;
        return true;
    }


    private void Hit()
    {
        Target.TakeDamage(Data.Damage);
    }
}
