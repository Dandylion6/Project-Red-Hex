using System.Collections.Generic;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    private Dictionary<Vector2Int, HexTile> tileMap = new();


    public void SetTileMap(Dictionary<Vector2Int, HexTile> newTileMap) => tileMap = newTileMap;

    public HexTile GetTile(Vector2Int axialCoordinate) => tileMap[axialCoordinate];
    public HexTile GetTile(int q, int r) => GetTile(new(q, r));


}
