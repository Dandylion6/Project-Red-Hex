using System;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : Singleton<HotBar>
{
    [Header("References")]
    [SerializeField] private Transform hotbarParent = null;
    [SerializeField] private List<Item> items = new();

    public IReadOnlyList<Item> Items => items;
    public Item CurrentItem => currentItem;

    private Action<Item, Item> onSelectionChanged = null; // has current item and last item as parameters.
    private Item currentItem = null;
    private TilePiece player = null;


    public void SubscribeToOnSelectionChanged(Action<Item, Item> callback) => onSelectionChanged += callback;
    public void UnsubscribeFromOnSelectionChanged(Action<Item, Item> callback) => onSelectionChanged -= callback;


    public void SelectItem(Item item)
    {
        if (!TurnManager.Instance.HasTurn(player)) return;
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
        if (item.CooldownLeft > 0) return;
        TurnManager.Instance.SetState(TurnManager.State.UseItem);

        Item lastItem = currentItem;
        currentItem = item;
        onSelectionChanged?.Invoke(item, lastItem);
    }


    private void ChangeTarget(Item item)
    {
        // Deselect
        if (currentItem == item)
        {
            DeselectCurrentItem();
            return;
        }
        SetAsTarget(item);
    }


    private void DeselectCurrentItem()
    {
        TurnManager.Instance.SetState(TurnManager.State.None);

        Item lastItem = currentItem;
        currentItem = null;
        onSelectionChanged?.Invoke(null, lastItem);
        HexGridManager.Instance.ClearOverlay();
    }


    private void Start() => player = GameManager.Instance.Player;


    private void Update()
    {
        // When it isn't the player's turn they can't choose any item to use.
        if (!TurnManager.Instance.HasTurn(player) && currentItem != null)
            DeselectCurrentItem();
    }
}
