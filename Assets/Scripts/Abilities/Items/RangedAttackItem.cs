using System.Collections;
using UnityEngine;

public abstract class RangedAttackItem<T> : Item<T> where T : RangedAttackData
{
    protected IDamageable Target => target;

    private IDamageable target = null;


    protected override void OnItemSelected()
    {
        HexGridManager.DisplayType displayType = Data.IgnoresObstacles ? HexGridManager.DisplayType.Basic : HexGridManager.DisplayType.Attack;
        HexGridManager.Instance.DisplayRange(Player.Occupying, Data.AttackRange, displayType);
    }


    protected override void OnItemDeselected() => target = null;


    protected override void OnTileSelect(HexTile tile)
    {
        if (!CanAttack(tile, out IDamageable damageable)) return;

        target = damageable;
        StartCoroutine(ActionSequence());
    }


    protected virtual bool CanAttack(HexTile tile, out IDamageable damageable)
    {
        damageable = null;

        // Make sure the tile actually has something worth damaging.
        if (tile.Piece == null) return false;
        if (tile.Piece is not IDamageable result) return false;
        if (tile.Piece == Player) return false;

        damageable = result;

        // Check if the tile is in range.
        if (Hexagon.HexDistance(Player.Occupying, tile) > Data.AttackRange) return false;
        if (!Data.IgnoresObstacles && !HexGridManager.Instance.InLineOfSight(Player.Occupying, tile)) return false;
        return true;
    }

    protected override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();

        yield return new WaitForSeconds(0.5f);

        target.TakeDamage(Data.Damage);
        TurnManager.Instance.EndTurn();
    }
}


public class RangedAttackItem : RangedAttackItem<RangedAttackData>
{

}