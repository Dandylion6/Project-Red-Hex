using System.Collections.Generic;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    private Dictionary<Vector2Int, HexTile> tileMap = new();
    private Pathfinding pathfinding = null;


    public void SetTileMap(Dictionary<Vector2Int, HexTile> newTileMap)
    {
        tileMap = newTileMap;
        pathfinding = new(tileMap);
    }


    public HexTile GetTile(Vector2Int axialCoordinate) => tileMap[axialCoordinate];
    public HexTile GetTile(int q, int r) => GetTile(new(q, r));


    public Pathfinding.Result CalculatePath(HexTile start, HexTile end)
    {
        return pathfinding.CalculatePath(start, end);
    }
}
