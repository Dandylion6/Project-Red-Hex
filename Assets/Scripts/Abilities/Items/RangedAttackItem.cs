using UnityEngine;

public class RangedAttackItem : Item<RangedAttackData>
{
    protected override void OnItemSelected()
    {
        HexGridManager.DisplayType displayType = Data.IgnoresObstacles ? HexGridManager.DisplayType.Basic : HexGridManager.DisplayType.Attack;
        HexGridManager.Instance.DisplayRange(Player.Occupying, Data.AttackRange, displayType);
    }


    protected override void OnItemDeselected()
    {
        HexGridManager.Instance.ClearOverlay();
    }

    
    protected override void OnTileSelect(HexTile tile)
    {
        
    }
}
