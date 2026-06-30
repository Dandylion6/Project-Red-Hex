using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combo Attack", menuName = "Data/Items/Combo Attack")]
public class ComboAttackData : RangedAttackData
{
    [Header("Combo Data")]
    [SerializeField][Tooltip("Chance to hit in percentage.")] private float hitChance = 40.0f;
    [SerializeField] private int minHits = 1;
    [SerializeField] private int maxHits = 3;


    public float HitChance => hitChance;
    public int MinHits => minHits;
    public int MaxHits => maxHits;


    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new("Hits", MinHits.ToString() + " -> " + MaxHits.ToString()));
        entries.Enqueue(new("Hit Chance", HitChance.ToString() + "%"));

        return entries;
    }
}
