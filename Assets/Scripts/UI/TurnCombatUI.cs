using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnCombatUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private HotBarSlotUI prefabSlot = null;
    [SerializeField] private RectTransform slotParent = null;
    [SerializeField] private TMP_Text turnCounter = null;
    [SerializeField] private CanvasGroup descriptionBox = null;
    [SerializeField] private TMP_Text itemNameText = null;
    [SerializeField] private TMP_Text descriptionText = null;
    [SerializeField] private TMP_Text statsText = null;


    private HotBar hotBar = null;


    private void Start()
    {
        if (!GameManager.Instance.Player.TryGetComponent(out hotBar)) return;

        foreach (Item item in hotBar.Items)
        {
            HotBarSlotUI slotUI = Instantiate(prefabSlot, slotParent);
            slotUI.Initialize(hotBar, item);
        }

        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
        HotBar.Instance.SubscribeToOnSelectionChanged(OnSelectionChanged);

        canvas.enabled = false;
        descriptionBox.alpha = 0.0f;
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        bool stateChanged = TurnManager.Instance.IsInCombat != canvas.enabled;
        if (stateChanged)
            canvas.enabled = TurnManager.Instance.IsInCombat;

        turnCounter.text = "~ Turn " + TurnManager.Instance.TurnsInCombat + " ~";
    }


    private void OnSelectionChanged(Item currentItem, Item lastItem)
    {
        if (currentItem == null)
        {
            descriptionBox.alpha = 0.0f;
            return;
        }

        descriptionBox.alpha = 1.0f;
        itemNameText.text = currentItem.BaseData.DisplayName;
        descriptionText.text = currentItem.BaseData.Description;

        statsText.text = string.Empty;
        Queue<ItemStatEntry> entries = currentItem.BaseData.GetStats();
        while (entries.Count > 0)
        {
            ItemStatEntry entry = entries.Dequeue();
            statsText.text += entry.label + " - " + entry.value + "\n";
        }
    }


    private void OnDestroy()
    {
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
        HotBar.Instance.UnsubscribeFromOnSelectionChanged(OnSelectionChanged);
    }
}
