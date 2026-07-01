using UnityEngine;


public class ConsumableTile : PickupTile
{
    [Header("Consumable Settings")]
    [SerializeField] private ConsumableItemData consumable = null;

    protected override void OnPickup()
    {
        if (!HotBar.Instance.AddConsumable(consumable)) return;
        Destroy(ToRemove);
    }
}
