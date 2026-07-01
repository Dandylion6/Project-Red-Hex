using UnityEngine;


public class ConsumableTile : MonoBehaviour
{
    [Header("Consumable Settings")]
    [SerializeField] private ConsumableItemData consumable = null;
    [SerializeField] private GameObject toPickUp = null;


    private HexTile tile = null;


    void Start()
    {
        if (!TryGetComponent(out tile))
        {
            Destroy(this);
            return;
        }

        tile.SubscribeToOnPiecePlaced(OnPiecePlaced);
    }


    private void OnPiecePlaced(TilePiece piece)
    {
        if (piece != GameManager.Instance.Player) return;
        if (!HotBar.Instance.AddConsumable(consumable)) return;
        Destroy(toPickUp);
    }


    private void OnDestroy()
    {
        if (tile != null)
            tile.UnsubscribeFromOnPiecePlaced(OnPiecePlaced);
    }
}
