using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(HexTile))]
public abstract class PickupTile : MonoBehaviour
{
    [Header("References")]
    [SerializeField][Tooltip("Will be removed when picked up.")] private GameObject toRemove = null;


    protected GameObject ToRemove => toRemove;

    private HexTile tile = null;
    private Vector3 idlePosition = Vector3.zero;


    protected abstract void OnPickup();


    private void Start()
    {
        idlePosition = toRemove.transform.position;

        if (!TryGetComponent(out tile)) return;
        tile.SubscribeToOnPiecePlaced(OnPiecePlaced);
        tile.SubscribeToOnPieceRemoved(OnPieceRemoved);
    }


    private void OnPiecePlaced(TilePiece piece)
    {
        if (toRemove == null)
        {
            Destroy(this);
            return;
        }
        toRemove.transform.DOKill();
        if (piece != GameManager.Instance.Player)
        {
            toRemove.transform.DOMoveY(idlePosition.y + piece.HealthBarHeight, 0.5f).SetEase(Ease.OutBack).Play();
            return;
        }
        
        toRemove.transform.DOMoveY(idlePosition.y + piece.HealthBarHeight, 0.5f).SetEase(Ease.OutBack).OnComplete(OnPickup).Play();
    }


    private void OnPieceRemoved(TilePiece piece)
    {
        if (toRemove == null)
        {
            Destroy(this);
            return;
        }
        toRemove.transform.DOKill();
        toRemove.transform.DOMoveY(idlePosition.y, 0.6f).SetEase(Ease.InOutBack).Play();
    }


    private void OnDestroy()
    {
        if (tile == null) return;
        tile.UnsubscribeFromOnPiecePlaced(OnPiecePlaced);
        tile.UnsubscribeFromOnPieceRemoved(OnPieceRemoved);
    }
}
