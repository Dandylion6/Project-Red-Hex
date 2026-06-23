using UnityEngine;

public class PlacedPiece : TilePiece
{
    private void Start()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        HexTile tile = HexGridManager.Instance.GetTile(hexAxial);

        if (tile == null) return;

        transform.position = tile.transform.position;
        tile.SetPiece(this);
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
