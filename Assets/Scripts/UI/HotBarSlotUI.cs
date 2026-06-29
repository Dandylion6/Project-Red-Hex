using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image slot = null;
    [SerializeField] private Image itemIcon = null;
    [SerializeField] private GameObject cooldown = null;
    [SerializeField] private TMP_Text cooldownTimer = null;

    [Header("Animation Settings")]
    [SerializeField] private float selectionOffset = 10.0f;
    [SerializeField] private float selectionSizeChange = 4.0f;
    [SerializeField] private Color selectionTint = Color.white;
    [SerializeField] private float animationTime = 0.4f;


    private Item item = null;
    private Tween selectionLoop = null;
    private Tween selectionEnd = null;

    private float idleHeight = 0.0f;
    private float selectedHeight = 0.0f;


    public void Initialize(Item item)
    {
        this.item = item;

        itemIcon.sprite = item.BaseData.ItemSprite;
        cooldown.SetActive(false);

        HotBar.Instance.SubscribeToOnSelectionChanged(OnSelectionChanged);

        idleHeight = slot.rectTransform.anchoredPosition.y;
        selectedHeight = idleHeight + selectionOffset;

        selectionEnd = slot.rectTransform.DOSizeDelta(slot.rectTransform.sizeDelta, animationTime * 0.6f).SetEase(Ease.OutBack).SetAutoKill(false).Pause();

        Vector2 newSize = slot.rectTransform.sizeDelta + Vector2.one * selectionSizeChange;
        selectionLoop = slot.rectTransform.DOSizeDelta(newSize, animationTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).Pause();
    }


    public void OnButtonClick() => HotBar.Instance.SelectItem(item);


    private void OnSelectionChanged(Item currentItem, Item lastItem)
    {
        if (currentItem == item)
        {
            slot.DOColor(selectionTint, animationTime * 0.6f).SetEase(Ease.OutQuad).Play();
            slot.rectTransform.DOAnchorPosY(selectedHeight, animationTime * 0.6f).SetEase(Ease.OutBack).Play();
            selectionEnd.Pause();
            selectionLoop.Restart();
        }
        else if (lastItem == item)
        {
            slot.DOColor(Color.white, animationTime * 0.6f).SetEase(Ease.OutQuad).Play();
            slot.rectTransform.DOAnchorPosY(idleHeight, animationTime * 0.6f).SetEase(Ease.OutBack).Play();
            selectionLoop.Pause();
            selectionEnd.Restart();
        }
    }


    private void Update()
    {
        if (!TurnManager.Instance.HasTurn(GameManager.Instance.Player)) return;
        cooldown.SetActive(item.CooldownLeft > 0);
        cooldownTimer.text = item.CooldownLeft.ToString();
    }


    private void OnDestroy() => HotBar.Instance.UnsubscribeFromOnSelectionChanged(OnSelectionChanged);
}
