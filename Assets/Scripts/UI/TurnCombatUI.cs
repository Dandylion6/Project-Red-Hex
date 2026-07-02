using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnCombatUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private HotBarSlotUI prefabSlot = null;
    [SerializeField] private RectTransform slotParent = null;
    [Space]
    [SerializeField] private RectTransform turnPivot = null;
    [SerializeField] private TMP_Text currentTurnCounter = null;
    [SerializeField] private TMP_Text nextTurnCounter = null;
    [Space]
    [SerializeField] private CanvasGroup descriptionBox = null;
    [SerializeField] private TMP_Text itemNameText = null;
    [SerializeField] private TMP_Text descriptionText = null;
    [SerializeField] private TMP_Text statsText = null;

    [Header("Turn Counter Settings")]
    [SerializeField] private float turnStartHeight = -40.0f;
    [SerializeField] private float turnEndHeight = 40.0f;

    [Header("Description Animation Settings")]
    [SerializeField] private float animationTime = 0.4f;


    private Tween turnRotate = null;
    private int currentTurnIndex = 0;


    private void Start()
    {
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
        HotBar.Instance.SubscribeToOnSelectionChanged(OnSelectionChanged);

        foreach (Item item in HotBar.Instance.Items)
        {
            HotBarSlotUI slotUI = Instantiate(prefabSlot, slotParent);
            slotUI.Initialize(item);
        }

        turnRotate = turnPivot.DOAnchorPosY(turnEndHeight, 1.0f).SetEase(Ease.InOutBack).SetAutoKill(false).Pause();
        canvas.alpha = 0.0f;
        canvas.interactable = false;
        descriptionBox.alpha = 0.0f;
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        if (TurnManager.Instance.IsInCombat)
        {
            canvas.DOKill();
            canvas.DOFade(1.0f, 0.3f).SetEase(Ease.OutSine).OnComplete(() => canvas.interactable = true).Play();
        }
        else
        {
            canvas.DOKill();
            canvas.DOFade(0.0f, 0.45f).SetEase(Ease.InOutSine).Play();
            canvas.interactable = false;
        }
        
        if (currentTurnIndex == TurnManager.Instance.TurnsInCombat) return;

        currentTurnCounter.text = "- Turn " + currentTurnIndex + " -";
        nextTurnCounter.text = "- Turn " + TurnManager.Instance.TurnsInCombat + " -";

        Vector2 position = turnPivot.anchoredPosition;
        position.y = turnStartHeight;
        turnPivot.anchoredPosition = position;
        turnRotate.Restart();

        turnPivot.DOScale(0.9f, 0.1f).SetEase(Ease.OutSine).Play();
        turnPivot.DOScale(1.0f, 0.1f).SetEase(Ease.InSine).SetDelay(0.9f).Play();
        currentTurnIndex = TurnManager.Instance.TurnsInCombat;
    }


    private void OnSelectionChanged(Item currentItem, Item lastItem)
    {
        if (currentItem == null)
        {
            descriptionBox.DOFade(0.0f, animationTime).SetEase(Ease.InOutQuad).Play();
            return;
        }

        itemNameText.text = currentItem.BaseData.DisplayName;
        descriptionText.text = currentItem.BaseData.Description;
        statsText.text = string.Empty;

        Queue<ItemStatEntry> entries = currentItem.BaseData.GetStats();
        while (entries.Count > 0)
        {
            ItemStatEntry entry = entries.Dequeue();
            statsText.text += entry.label + " - " + entry.value + "\n";
        }

        descriptionBox.DOFade(1.0f, animationTime).SetEase(Ease.InOutQuad).Play();
    }


    private void OnDestroy()
    {
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
        HotBar.Instance.UnsubscribeFromOnSelectionChanged(OnSelectionChanged);
    }
}