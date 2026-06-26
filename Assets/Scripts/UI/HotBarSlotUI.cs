using UnityEngine;

public class HotBarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform slot = null;

    [Header("Button Settings")]
    [SerializeField] private float selectionOffset = 10.0f;
    

    private HotBar hotBar = null;
    private Item item = null;


    public void Initialize(HotBar hotBar, Item item)
    {
        this.hotBar = hotBar;
        this.item = item;

        hotBar.SubscribeToOnSelectionChanged(OnSelectionChanged);
        OnSelectionChanged(item, null); // Syncing to current.
    }


    public void OnButtonClick() => hotBar.SelectItem(item);


    private void OnSelectionChanged(Item currentItem, Item lastItem)
    {
        float offset = currentItem == this.item ? selectionOffset : -selectionOffset;
        slot.anchoredPosition += Vector2.up * offset;
    }


    private void OnDestroy()
    {
        if (hotBar != null)
            hotBar.UnsubscribeFromOnSelectionChanged(OnSelectionChanged);
    }
}
