using System.Collections;
using UnityEngine;

public abstract class RangedAttackItem<T> : Item<T> where T : RangedAttackData
{
    protected IDamageable Target => target;
    protected HexTile TargetTile => targetTile;

    private IDamageable target = null;
    private HexTile targetTile = null;


    protected override void OnItemSelected()
    {
        HexGridManager.DisplayType displayType = Data.IgnoresObstacles ? HexGridManager.DisplayType.Basic : HexGridManager.DisplayType.Attack;
        HexGridManager.Instance.DisplayRange(Player.Occupying, Data.AttackRange, displayType);
    }


    protected override void OnItemDeselected() { }


    protected override void OnTileSelect(HexTile tile)
    {
        if (!CanAttack(tile, out IDamageable damageable)) return;

        target = damageable;
        targetTile = tile;

        Player.RotateTo(tile);
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
        
        if (Data.Effect != null)
        {
            EffectSequence sequence = Instantiate(Data.Effect);

            float rotation = 120.0f - Player.Rotation;
            bool lookingRight = Player.Rotation >= 30.0f && Player.Rotation <= 210.0f;
            sequence.transform.localScale = new(1.0f, lookingRight ? 1.0f : -1.0f, 1.0f);
            sequence.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);

            yield return sequence.PlaySeqeunce(Player.transform);
        }

        target.TakeDamage(Data.Damage, AudioManager.Instance.AudioBank.MusketHit);

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }
}


public class RangedAttackItem : RangedAttackItem<RangedAttackData>
{

}