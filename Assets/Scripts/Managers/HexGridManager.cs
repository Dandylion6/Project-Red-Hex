using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    public int TileCount => tileMap.Count;

    private Dictionary<Vector2Int, HexTile> tileMap = new();
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
}
