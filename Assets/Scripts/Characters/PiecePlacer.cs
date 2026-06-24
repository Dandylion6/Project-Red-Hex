using UnityEngine;

[RequireComponent(typeof(TilePiece))]
public class PiecePlacer : MonoBehaviour
{
    private void PlacePiece()
    {
        TilePiece piece = GetComponent<TilePiece>();
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        HexTile tile = HexGridManager.Instance.GetTile(hexAxial);

        if (tile == null) return;

        piece.SpawnAt(tile);
        Destroy(this); // Not needed anymore.
    }


    private void Update()
    {
        if (HexTileMapBinder.Instance.IsSetup)
            PlacePiece();
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected() => SnapToGrid();


    private void SnapToGrid()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        Vector2 world = Hexagon.AxialToWorld(hexAxial);
        transform.position = new(world.x, transform.position.y, world.y);
    }
#endif
}
