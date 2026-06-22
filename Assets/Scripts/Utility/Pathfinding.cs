using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding
{
    private const int MAX_ITERATIONS = 1000;


    public struct Result
    {
        public Stack<HexTile> path;
        public int tileDistance;
        public bool isComplete;
    }


    private readonly struct Node
    {
        public readonly int g, h, f;

        public Node(int g, int h)
        {
            this.g = g;
            this.h = h;
            f = g + h;
        }
    }


    private readonly Dictionary<Vector2Int, HexTile> tileMap = null;


    public Pathfinding(Dictionary<Vector2Int, HexTile> tileMap)
    {
        this.tileMap = tileMap;
    }


    public Result CalculatePath(HexTile start, HexTile end)
    {
        Result result = new();

        List<HexTile> canCheck = new() { start };

        Dictionary<HexTile, HexTile> cameFrom = new();
        Dictionary<HexTile, Node> nodes = new()
        {
            { start, new(0, GetHScore(start, end)) }
        };

        for (int i = 0; i < MAX_ITERATIONS; ++i)
        {
            HexTile current = GetTileWithLowestF(canCheck, nodes);
            if (current == end) // Finnished path.
            {
                result.path = ReconstructPath(cameFrom, current);
                result.tileDistance = result.path.Count;
                result.isComplete = true;
            }

            canCheck.Remove(current);
            HexTile[] neighbors = GetNeighboringTiles(start);
            for (int j = 0; j < neighbors.Length; ++j)
            {
                HexTile neighbor = neighbors[j];
                if (neighbor == null) continue;
                if (!neighbor.IsWalkable) continue;

                int tentativeG = nodes[current].g + 1;
                int g = nodes.ContainsKey(neighbor) ? nodes[neighbor].g : 0;
                if (tentativeG >= g) continue;

                // Record the better path.
                Node newNode = new(tentativeG, GetHScore(neighbor, end));
                nodes.Add(neighbor, newNode);
                cameFrom[neighbor] = current;

                if (!canCheck.Contains(neighbor))
                    canCheck.Add(neighbor);
            }
        }

        return result;
    }


    private int GetHScore(HexTile tile, HexTile end)
    {
        float distance = Vector3.Distance(tile.transform.position, end.transform.position);
        return Mathf.RoundToInt(distance);
    }


    private HexTile GetTileWithLowestF(List<HexTile> canCheck, Dictionary<HexTile, Node> nodes)
    {
        if (canCheck.Count == 0) return null;

        HexTile withLowestF = canCheck[0];
        int lowestF = nodes[withLowestF].f;

        foreach(HexTile check in canCheck)
        {
            int f = nodes[check].f;
            if (lowestF <= f) continue;

            withLowestF = check;
            lowestF = f;
        }

        return withLowestF;
    }


    private Stack<HexTile> ReconstructPath(Dictionary<HexTile, HexTile> cameFrom, HexTile current)
    {
        Stack<HexTile> totalPath = new();
        while (current != null)
        {
            current = cameFrom[current];
            totalPath.Push(current);
        }
        return totalPath;
    }


    private HexTile[] GetNeighboringTiles(HexTile tile)
    {
        HexTile[] neighbors = new HexTile[6];

        if (tileMap.TryGetValue(tile.AxialCoordinate + Vector2Int.up, out HexTile up)) neighbors[0] = up;
        if (tileMap.TryGetValue(tile.AxialCoordinate + Vector2Int.down, out HexTile down)) neighbors[1] = down;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(-1, 1), out HexTile topLeft)) neighbors[2] = topLeft;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(-1, -1), out HexTile bottomLeft)) neighbors[3] = bottomLeft;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(1, 1), out HexTile topRight)) neighbors[4] = topRight;
        if (tileMap.TryGetValue(tile.AxialCoordinate + new Vector2Int(1, -1), out HexTile bottomRight)) neighbors[5] = bottomRight;

        return neighbors;
    }
}
