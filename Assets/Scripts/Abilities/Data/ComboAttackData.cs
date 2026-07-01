using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ComboData
{
    public float hitChance;
    public float damageMultiplier;
}


[CreateAssetMenu(fileName = "New Combo Attack", menuName = "Data/Items/Combo Attack")]
public class ComboAttackData : RangedAttackData
{
    [Header("Combo Data")]
    [SerializeField] private List<ComboData> combo = new();


    public IReadOnlyList<ComboData> Combo => combo;


    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new("Hits", Combo.Count.ToString()));

        return entries;
    }
}
