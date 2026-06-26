using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Attack", menuName = "Data/Items/RangedAttack")]
public class RangedAttackData : ItemData
{
    [Header("Ranged Attack Data")]
    [SerializeField][Min(1)] private int damage = 1;
    [SerializeField][Min(1)][Tooltip("Amoujnt of hexagon distance the attack can reach.")] private int attackRange = 2;
    [SerializeField] private bool ignoresObstacles = false;


    public int Damage => damage;
    public int AttackRange => attackRange;
    public bool IgnoresObstacles => ignoresObstacles;


    public override Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = base.GetStats();

        entries.Enqueue(new("Damage", Damage.ToString()));
        entries.Enqueue(new("Range", AttackRange.ToString()));
        
        return entries;
    }
}
