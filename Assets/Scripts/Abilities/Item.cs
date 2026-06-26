using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for all items. Extend via <see cref="Item{T}"/> with a matching <see cref="ItemData"/> type.
/// Handles tile selection and hotbar event wiring automatically.
/// </summary>
public abstract class Item : MonoBehaviour
{
    /// <summary>
    /// Determines how the item selects its target tile.
    /// </summary>
    public enum CastType
    {
        OnEnemy,        // Will be able to hit any enemy (regardless of obstruction) within a range.
        OnVisibleEnemy, // Will only hit within range if not obstructed.
        OnPlayer,       // Will be used on the player tile.
    }


    protected abstract ItemData BaseData {  get; } // Will be overriden by T type.
    protected TilePiece Player => player;

    private TilePiece player = null;


    /// <summary>Called when the player selects a tile while this item is selected.</summary>
    protected abstract void OnTileSelect(HexTile tile);

    /// <summary>Called when this item becomes the active hotbar selection.</summary>
    protected abstract void OnItemSelected();

    /// <summary>Called when this item is deselected. Use to clean up any visuals shown in <see cref="OnItemSelected"/>.</summary>
    protected abstract void OnItemDeselected();

    protected abstract IEnumerator ActionSequence();
    

    public void Start()
    {
        player = GameManager.Instance.Player;
        HotBar.Instance.SubscribeToOnSelectionChanged(OnItemSelectionChanged);
    }


    private void OnItemSelectionChanged(Item currentItem, Item lastItem)
    {
        bool isSelected = currentItem != null;
        bool isCurrentItem = currentItem == this;

        if (isSelected && isCurrentItem)
        {
            TileSelect.Instance.SubscribeToOnSelectionChanged(OnTileSelect);
            OnItemSelected();
            return;
        }

        bool isLastItem = lastItem == this;
        if (!isSelected && isLastItem)
        {
            TileSelect.Instance.UnsubscibeFromOnSelectionChanged(OnTileSelect);
            OnItemDeselected();
        }
    }


    private void OnDestroy() => TileSelect.Instance.UnsubscibeFromOnSelectionChanged(OnTileSelect);
}


/// <summary>
/// Concrete item base. Assign a matching <typeparamref name="T"/> asset to the <c>data</c>
/// field in the Inspector, then implement the abstract members.
/// </summary>
public abstract class Item<T> : Item where T : ItemData
{
    [SerializeField] T data = null;


    protected T Data => data;
    protected override ItemData BaseData => data;
}