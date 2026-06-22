using UnityEngine;

public class TilePiece : MonoBehaviour
{
    public bool HasTurn => hasTurn;

    private HexTile occupying = null;
    private bool hasTurn = false;


    public void MoveTo(Vector3 target)
    {
        Vector2Int axialCoordinate = Hexagon.WorldToAxial(new(target.x, target.z));
        HexTile tile = HexGridManager.Instance.GetTile(axialCoordinate);
        MoveTo(tile);
    }


    public void MoveTo(HexTile tile)
    {
        if (!tile.CanSetPiece(this)) return;
        if (occupying != null) 
            occupying.RemovePiece();
        
        transform.position = tile.transform.position;
        
        tile.SetPiece(this);
        occupying = tile;
    }


    public void StartTurn() => hasTurn = true;
    public void EndTurn() => hasTurn = false;
}
