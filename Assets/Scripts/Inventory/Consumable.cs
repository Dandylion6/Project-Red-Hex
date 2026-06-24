using Unity.Hierarchy;
using UnityEngine;

public class Consumable : Item
{
    private float count; 

    public float getCount => count;

    public override bool Use()
    {
        count --;

        //consumable functionality
        return false;
    }
}
