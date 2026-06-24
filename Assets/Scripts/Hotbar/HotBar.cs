using System;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Item> items;


    public IReadOnlyList<Item> Items => items;
    public Item CurrentItem => currentItem;

    private Action<Item> onSelectionChanged = null;
    private Item currentItem = null;
    private TilePiece player = null;


    public void SubscribeToOnSelectionChanged(Action<Item> callback) => onSelectionChanged += callback;
    public void UnsubscribeFromOnSelectionChanged(Action<Item> callback) => onSelectionChanged -= callback;


    public void SelectItem(Item item)
    {
        switch (TurnManager.Instance.CurrentState)
        {
            case TurnManager.State.None: SetAsTarget(item);
                break;
            case TurnManager.State.UseItem: ChangeTarget(item);
                break;
        }
    }


    private void SetAsTarget(Item item)
    {
        if (!TurnManager.Instance.IsPeiceWithTurn(player)) return;
        
        TurnManager.Instance.SetState(TurnManager.State.UseItem);
        currentItem = item;

        onSelectionChanged?.Invoke(item);
        currentItem.Use();
    }


    private void ChangeTarget(Item item)
    {
        // Deselect
        if (currentItem == item)
        {
            DeselectCurrentItem();
            return;
        }

        TurnManager.Instance.SetState(TurnManager.State.UseItem);
        currentItem = item;
        onSelectionChanged?.Invoke(currentItem);
    }


    private void DeselectCurrentItem()
    {
        TurnManager.Instance.SetState(TurnManager.State.None);
        currentItem = null;
        onSelectionChanged?.Invoke(null);
    }


    private void Start() => player = GameManager.Instance.Player;


    private void Update()
    {
        // When it isn't the player's turn they can't choose any item to use.
        if (!TurnManager.Instance.IsPeiceWithTurn(player) && currentItem != null)
            DeselectCurrentItem();
    }
}
