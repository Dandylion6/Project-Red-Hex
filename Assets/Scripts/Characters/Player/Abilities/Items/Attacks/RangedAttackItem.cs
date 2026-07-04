using System.Collections;

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
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.MusketFire, SettingsManager.Instance.GameVolume , true, Player.transform.position);
        yield return TurnManager.TurnWait;

        target.TakeDamage(Data.Damage, AudioManager.Instance.AudioBank.MusketHit);
         
        target = null;

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }
}


public class RangedAttackItem : RangedAttackItem<RangedAttackData>
{

}