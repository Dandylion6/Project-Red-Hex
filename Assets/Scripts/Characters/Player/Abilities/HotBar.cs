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


    public void ResetAllCooldowns()
    {
        foreach (Item items in items) items.ResetCooldown();
    }


    public bool AddConsumable(ConsumableItemData consumable)
    {
        IConsumable item = FindConsumable(consumable);
        if (item == null) return false;
        
        item.AddConsumable();
        return true;
    }


    private IConsumable FindConsumable(ConsumableItemData consumable)
    {
        foreach (Item item in items)
        {
            if (item.BaseData == consumable)
                return item as IConsumable;
        }
        return null;
    }


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

        if (item.BaseData.ItemSelectSound != null)
            AudioManager.Instance.PlayOneShot(item.BaseData.ItemSelectSound);
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
        if (TurnManager.Instance.CurrentState != TurnManager.State.UseItem) return;
        TurnManager.Instance.SetState(TurnManager.State.None);

        Item lastItem = currentItem;
        currentItem = null;
        onSelectionChanged?.Invoke(null, lastItem);
        HexGridManager.Instance.ClearOverlay();
    }


    private void Start()
    {
        player = GameManager.Instance.Player;
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        if (!TurnManager.Instance.IsInCombat)
        {
            DeselectCurrentItem();
            return;
        }
        if (currentPiece != player) DeselectCurrentItem();
    }


    private void OnDestroy()
    {
        if (TurnManager.Instance == null) return;
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }
}
