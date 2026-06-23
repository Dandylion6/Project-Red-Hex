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
        switch (TurnManager.Instance.CurrentState) 
        {
            case TurnManager.State.None:
                {

                    TurnManager.Instance.SetState(TurnManager.State.UseItem);
                    isItemSelected = true;
                    currentItem = hotBar[index];

                    Debug.Log("Current item: " + currentItem);
                    Debug.Log("item mode" + isItemSelected);
                    break;
                }

            case TurnManager.State.UseItem:
                {
                    //DESELECT
                    if (currentItem == hotBar[index] && isItemSelected)
                    {
                        isItemSelected = false;
                        TurnManager.Instance.SetState(TurnManager.State.None);

                        Debug.Log("Current item: " + currentItem);
                        Debug.Log("item mode" + isItemSelected);

                        TurnManager.Instance.SetState(TurnManager.State.UseItem);
                        isItemSelected = true;
                        return;
                    }

                    currentItem = hotBar[index];
                    break;
                }
        }


       


    }

}
