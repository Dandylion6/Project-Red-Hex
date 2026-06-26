using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform slot = null;
    [SerializeField] private Image itemIcon = null;
    [SerializeField] private GameObject cooldown = null;
    [SerializeField] private TMP_Text cooldownTimer = null;

    [Header("Button Settings")]
    [SerializeField] private float selectionOffset = 10.0f;
    

    private HotBar hotBar = null;
    private Item item = null;


    public void Initialize(HotBar hotBar, Item item)
    {
        this.hotBar = hotBar;
        this.item = item;

        itemIcon.sprite = item.BaseData.ItemSprite;
        cooldown.SetActive(false);

        hotBar.SubscribeToOnSelectionChanged(OnSelectionChanged);
    }


    public void OnButtonClick() => hotBar.SelectItem(item);


    private void OnSelectionChanged(Item currentItem, Item lastItem)
    {
        float offset = currentItem == item ? selectionOffset : -selectionOffset;
        slot.anchoredPosition += Vector2.up * offset;
    }


    private void FixedUpdate()
    {
        cooldown.SetActive(item.CooldownLeft > 0);
        cooldownTimer.text = item.CooldownLeft.ToString();
    }


    private void OnDestroy()
    {
        if (hotBar != null)
            hotBar.UnsubscribeFromOnSelectionChanged(OnSelectionChanged);
    }
}
