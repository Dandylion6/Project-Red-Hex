using System.Collections;
using UnityEngine;

public class ConsumableItem : Item<ConsumableItemData>
{
    protected override IEnumerator ActionSequence()
    {
            StartCooldown();
            TurnManager.Instance.StartAction();
            Data.useConsumable();
            yield return new WaitForSeconds(0.5f);
    }

    protected override void OnTileSelect(HexTile tile)
    {
        if (canConsume(tile))
        {
            StartCoroutine(ActionSequence());
        }
        
    }

    public bool canConsume(HexTile tile)
    {
        if (tile.Piece == GameManager.Instance.Player && Data.getCount >= 1 && GameManager.Instance.Player.Health < GameManager.Instance.Player.MaxHealth)
            {
                return true;
            }
        else
        {
            return false;
        }
    }



    protected override void OnItemDeselected() => HexGridManager.Instance.ClearOverlay();

    protected override void OnItemSelected()
    {

        HexGridManager.DisplayType displayType = HexGridManager.DisplayType.Basic;
        HexGridManager.Instance.DisplayRange(Player.Occupying, 0, displayType);
    }
}
