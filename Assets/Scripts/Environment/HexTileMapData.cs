using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Data", menuName = "Data Objects/HexTileMapData")]
public class HexTileMapData : ScriptableObject
{
    private Dictionary<Vector2Int, HexTile> hexMap = new();
}
