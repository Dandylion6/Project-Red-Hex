using System;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private bool isWalkable = true;


    public TilePiece Piece => piece;
    public Vector2Int AxialCoordinate => Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
    public bool IsWalkable => isWalkable;

    private Action<TilePiece> onPiecePlaced = null;
    private TilePiece piece = null;


    public void SubscribeToOnPiecePlaced(Action<TilePiece> callback) => onPiecePlaced += callback;
    public void UnsubscibeToOnPiecePlaced(Action<TilePiece> callback) => onPiecePlaced -= callback;


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


    private void OnDrawGizmosSelected()
    {
        if (Application.isEditor && !Application.isPlaying) SnapToGrid();
    }


    private void SnapToGrid()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        Vector2 world = Hexagon.AxialToWorld(hexAxial);
        transform.position = new(world.x, transform.position.y, world.y);
    }
}
