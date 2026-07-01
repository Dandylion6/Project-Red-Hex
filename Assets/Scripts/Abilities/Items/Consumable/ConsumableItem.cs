using System;
using System.Collections;

public abstract class ConsumableItem<T> : Item<T>, IConsumable where T : ConsumableItemData
{
    private int count = 0;


    public void AddConsumable() => Data.SetCount(++count);

    public void UseConsumable() => Data.SetCount(--count);


    protected override IEnumerator ActionSequence()
    {
            StartCooldown();
            TurnManager.Instance.StartAction();
            Data.SetCount(--count);

            yield return TurnManager.TurnWait;
            TurnManager.Instance.EndTurn();
    }


    protected override void OnTileSelect(HexTile tile)
    {
        if (CanConsume(tile))
        {
            StartCoroutine(ActionSequence());
        }
        
    }


    protected bool CanConsume(HexTile tile)
    {
        if (tile == null) return false;
        if (tile.Piece != Player) return false;
        if (Data.Count <= 0) return false;

        return true;
    }


    protected override void OnItemDeselected() => HexGridManager.Instance.ClearOverlay();

    protected override void OnItemSelected()
    {

        HexGridManager.DisplayType displayType = HexGridManager.DisplayType.Basic;
        HexGridManager.Instance.DisplayRange(Player.Occupying, 0, displayType);

        Data.SetCount(count);
    }
}

public class ConsumableItem : ConsumableItem<ConsumableItemData>
{

}

