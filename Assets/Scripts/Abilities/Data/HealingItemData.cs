using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Berries", menuName = "Data/Items/Healing Item")]
public class HealingItemData : ConsumableItemData
{
    [SerializeField][Min(1)] private int healBy = 1;
    public int getHealBy => healBy;

    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new("Heal by", healBy.ToString()));

        return entries;
    }
}
