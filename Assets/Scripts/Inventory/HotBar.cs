using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    [SerializeField] private List<Item> hotBar;

    private Item currentItem;

    public void SelectItem(Item item)
    {
        currentItem = item;
    }

}
