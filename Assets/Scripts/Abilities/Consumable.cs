using Unity.Hierarchy;
using UnityEngine;

public class Consumable : Item
{
    private float count; 

    public float getCount => count;

    public override void Use()
    {
        count --;

        //consumable functionality
        
    }
}
