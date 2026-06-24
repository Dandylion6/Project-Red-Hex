using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject currentTile = null;
    [SerializeField] private List<GameObject> tileVariants = new(); 
    [SerializeField] private bool isWalkable = true;
    [SerializeField][Range(-0.15f, 0.15f)] private float heightOffset = 0.0f;
    [SerializeField] private bool ignoreHeight = false;


    public TilePiece Piece => piece;
    public HexOverlay Overlay => overlay;
    public Vector2Int AxialCoordinate => Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
    public Vector2 WorldPosition => new(transform.position.x, transform.position.z);
    public bool IsWalkable => isWalkable;
    public float HeightOffset => heightOffset;
    public bool IgnoreHeight => ignoreHeight;

    private HexOverlay overlay = null;
    private System.Action<TilePiece> onPiecePlaced = null;
    private TilePiece piece = null;


    public void SubscribeToOnPiecePlaced(System.Action<TilePiece> callback) => onPiecePlaced += callback;
    public void UnsubscibeToOnPiecePlaced(System.Action<TilePiece> callback) => onPiecePlaced -= callback;


    public bool TrySetPiece(TilePiece piece)
    {
        if (!CanSetPiece(piece)) return false;

        this.piece = piece;
        onPiecePlaced?.Invoke(piece);
        return true;
    }


    public void SetPiece(TilePiece piece)
    {
        this.piece = piece;
        onPiecePlaced?.Invoke(piece);
    }


    public TilePiece RemovePiece()
    {
        if (piece == null) return null;
        
        TilePiece oldPiece = piece;
        piece = null;

        return oldPiece;
    }


    public bool CanSetPiece(TilePiece piece)
    {
        if (piece == null) return false;
        if (!isWalkable) return false;
        if (this.piece != null) return false;

        // Check path.
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(piece.Occupying, this);
        if (!result.isComplete) return false;
        if (result.tileDistance > piece.MaxMoveDistance) return false;

        return true;
    }


    private void Start()
    {
        TryGetComponent(out overlay);

        if (tileVariants.Count == 0) return;
        int variantIndex = Random.Range(-1, tileVariants.Count);
        if (variantIndex == -1) return; // Will not change the tile.

        Instantiate(tileVariants[variantIndex], transform);
        if (currentTile != null) Destroy(currentTile);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected() => SnapToGrid();


    private void OnDrawGizmos()
    {
        Gizmos.color = isWalkable ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 0.2f);
    }


    private void SnapToGrid()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(WorldPosition);
        Vector2 world = Hexagon.AxialToWorld(hexAxial);
        transform.position = new(world.x, transform.position.y, world.y);
    }
#endif
}
