using UnityEngine;

public struct ItemStatEntry
{
    public string label;
    public Sprite statIcon; // Might not need.
    public string value;


    public ItemStatEntry(string label, string value, Sprite statIcon = null)
    {
        this.label = label;
        this.statIcon = statIcon;
        this.value = value;
    }
}
