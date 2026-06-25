using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    public int TileCount => tileMap.Count;

    private Dictionary<Vector2Int, HexTile> tileMap = new();
    private readonly List<HexTile> overlayTiles = new();
    private Pathfinding pathfinding = null;


    public void Setup(List<Vector2Int> axialCoordinates, List<HexTile> hexTiles)
    {
        tileMap = RebuildDictionary(axialCoordinates, hexTiles);
        pathfinding = new(tileMap);
    }


    private Dictionary<Vector2Int, HexTile> RebuildDictionary(List<Vector2Int> axialCoordinates, List<HexTile> hexTiles)
    {
        Dictionary<Vector2Int, HexTile> tileMap = new();
        for (int i = 0; i < axialCoordinates.Count; ++i)
        {
            Vector2Int coordinate = axialCoordinates[i];
            if (tileMap.ContainsKey(axialCoordinates[i])) continue; // Can't allow overlaps.

            HexTile tile = hexTiles[i];
            tileMap.Add(coordinate, tile);
        }
        return tileMap;
    }


    public HexTile GetTile(Vector2Int axialCoordinate) => tileMap[axialCoordinate];
    public HexTile GetTile(int q, int r) => GetTile(new(q, r));
    public Vector2Int GetKeyFromIndex(int index) => tileMap.ElementAt(index).Key;


    public Pathfinding.Result CalculatePath(HexTile start, HexTile end)
    {
        return pathfinding.CalculatePath(start, end);
    }


    public void DisplayRange(HexTile origin, int hexRange)
    {
        ClearOverlayOfType(HexOverlay.Type.Range);

        Queue<(HexTile tile, int distance)> open = new(); 
        HashSet<HexTile> visited = new();

        open.Enqueue((origin, 0));
        visited.Add(origin);

        while (open.Count > 0)
        {
            (HexTile tile, int distance) = open.Dequeue();

            if (tile.Overlay != null)
            {
                tile.Overlay.SetType(HexOverlay.Type.Range);
                overlayTiles.Add(tile);
            }

            if (distance >= hexRange)
                continue;

            HexTile[] neighbors = GetNeighboringTiles(tile);
            foreach (HexTile neighbor in neighbors)
            {
                if (neighbor == null) continue;
                if (visited.Add(neighbor))
                    open.Enqueue((neighbor, distance + 1));
            }
        }
    }


    public void ClearOverlayOfType(HexOverlay.Type type)
    {
        foreach(HexTile tile in overlayTiles)
        {
            if (tile.Overlay == null) continue;
            if (tile.Overlay.CurrentType == type)
                tile.Overlay.SetType(HexOverlay.Type.None);
        }
    }


    public HexTile[] GetNeighboringTiles(HexTile tile)
    {
        HexTile[] neighbors = new HexTile[6];

        if (tileMap.TryGetValue(tile.AxialCoordinate + Vector2Int.up, out HexTile up)) neighbors[0] = up;
        if (tileMap.TryGetValue(tile.AxialCoordinate + Vector2Int.down, out HexTile down)) neighbors[1] = down;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(-1, 1), out HexTile topLeft)) neighbors[2] = topLeft;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(-1, 0), out HexTile left)) neighbors[3] = left;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(1, 0), out HexTile right)) neighbors[4] = right;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(1, -1), out HexTile bottomRight)) neighbors[5] = bottomRight;

        return neighbors;
    }
}
