using UnityEngine;
using System.Collections.Generic;

public class ConsumableItemData : ItemData
{
    [Header("Consumable Data")]
    [SerializeField][Min(1)] private int healBy = 1;

    private int count = 0;

    public int getHealBy => healBy;

    public int getCount => count;

    public void useConsumable()
    {
        count--;
    }

    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new("Heal by", healBy.ToString()));

        return entries;
    }
}
