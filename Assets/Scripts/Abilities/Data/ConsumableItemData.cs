using UnityEngine;
using System.Collections.Generic;

public class ConsumableItemData : ItemData
{
    [Header("Consumable Data")]


    [SerializeField] private int count = 5;

    public int Count => count;

    public void SetCount(int count)
    {
        this.count = count;
    }

    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new ItemStatEntry("Amount", count.ToString()));
        return entries;
    }
}
