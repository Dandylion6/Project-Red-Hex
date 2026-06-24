using UnityEngine;

public class TurnCombatUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private HotBarSlotUI prefabSlot = null;
    [SerializeField] private RectTransform slotParent = null;

    private HotBar hotBar = null;


    private void Start()
    {
        if (!GameManager.Instance.Player.TryGetComponent(out hotBar)) return;

        foreach (Item item in hotBar.Items)
        {
            HotBarSlotUI slotUI = Instantiate(prefabSlot, slotParent);
            slotUI.Initialize(hotBar, item);
        }
    }


    private void Update()
    {
        bool stateChanged = TurnManager.Instance.IsInCombat != canvas.enabled;
        if (stateChanged)
            canvas.enabled = TurnManager.Instance.IsInCombat;
    }
}
