using System.Collections;
using UnityEngine;

public class ComboAttackItem : RangedAttackItem<ComboAttackData>
{
    protected override IEnumerator ActionSequence()
    {
        IDamageable target = Target;

        StartCooldown();
        TurnManager.Instance.StartAction();

        for (int i = 0; i < Data.MaxHits; ++i)
        {
            yield return new WaitForSeconds(0.5f);

            if (i < Data.MinHits)
            {
                Hit(target);
                continue;
            }

            if (!CanHit(target)) break;

            Hit(target);
        }

        TurnManager.Instance.EndTurn();
    }


    private bool CanHit(IDamageable damageable)
    {
        if (damageable == null) return false;

        float chance = Random.Range(0.0f, 100.0f);
        if (chance > Data.HitChance) return false;
        return true;
    }


    private void Hit(IDamageable target)
    {
        if (target == null) return;
        target.TakeDamage(Data.Damage);
    }
}
