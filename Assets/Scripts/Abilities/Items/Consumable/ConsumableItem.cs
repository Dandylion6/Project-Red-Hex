using System;
using System.Collections;
using UnityEngine;

public abstract class ConsumableItem<T> : Item<T> where T : ConsumableItemData
{
    private int count = 0;

    public void AddCount()
    {
        Data.SetCount(++count);
    }

    protected override IEnumerator ActionSequence()
    {
            StartCooldown();
            TurnManager.Instance.StartAction();
            Data.SetCount(--count);
            yield return new WaitForSeconds(0.5f);

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
        if (Data.Count >= 1 && tile.Piece == GameManager.Instance.Player)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void UseConsumable()
    {
        Data.SetCount(--count);
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

