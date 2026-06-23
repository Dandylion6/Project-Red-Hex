using UnityEngine;

[RequireComponent(typeof(TilePiece))]
public class PiecePlacer : MonoBehaviour
{
    private void Start()
    {
        TilePiece piece = GetComponent<TilePiece>();
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        HexTile tile = HexGridManager.Instance.GetTile(hexAxial);

        if (tile == null) return;

        piece.SpawnAt(tile);
        Destroy(this); // Not needed anymore.
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
