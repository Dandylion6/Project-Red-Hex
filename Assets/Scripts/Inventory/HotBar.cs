using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    [SerializeField] private List<Item> hotBar;

    [SerializeField] private bool isItemSelected = false;

    [SerializeField] private Item currentItem;

    public bool IsItemSelected => isItemSelected; 

    

    public void SelectItem(int index)
    {
        
        if (currentItem == hotBar[index] && isItemSelected) 
        {
            isItemSelected = false;

            Debug.Log("Current item: " + currentItem);
            Debug.Log("item mode" + isItemSelected);

            return;
        }

        isItemSelected = true;
        currentItem = hotBar[index];
        
        Debug.Log("Current item: " + currentItem);
        Debug.Log("item mode" + isItemSelected);
    }

}
