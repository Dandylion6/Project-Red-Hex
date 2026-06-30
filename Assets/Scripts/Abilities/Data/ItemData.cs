using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Data/Items/Item")]
public class ItemData : ScriptableObject
{
    [Header("Display Data")]
    [SerializeField] private string displayName = "New Item";
    [SerializeField] private Sprite itemSprite = null;
    [SerializeField][TextArea] private string description = "Short description of the item.";

    [Header("Item Data")]
    [SerializeField][Min(0)] private int cooldown = 1;


    public string DisplayName => displayName;
    public Sprite ItemSprite => itemSprite;
    public string Description => description;
    public int Cooldown => cooldown;


    public virtual Queue<ItemStatEntry> GetStats()
    {
        Queue<ItemStatEntry> entries = new();
        if (cooldown > 0) entries.Enqueue(new("Cooldown", Cooldown.ToString()));
        return entries;
    }
}
