using System.Collections.Generic;
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

        List<HexTile> openTiles = new() { start };
        HashSet<HexTile> closedTiles = new();

        Dictionary<HexTile, HexTile> cameFrom = new();
        Dictionary<HexTile, Node> nodes = new()
        {
            { start, new(0, GetHScore(start, end)) }
        };

        for (int i = 0; i < MAX_ITERATIONS; ++i)
        {
            if (openTiles.Count == 0) break;

            HexTile current = GetTileWithLowestF(openTiles, nodes);
            if (current == end) // Finnished path.
            {
                result.path = ReconstructPath(cameFrom, current);
                result.tileDistance = Mathf.Max(result.path.Count - 1, 0);
                result.isComplete = true;
                return result;
            }

            openTiles.Remove(current);
            closedTiles.Add(current);

            HexTile[] neighbors = HexGridManager.Instance.GetNeighboringTiles(current);
            for (int j = 0; j < neighbors.Length; ++j)
            {
                HexTile neighbor = neighbors[j];
                if (neighbor == null) continue;

                if (!neighbor.IsWalkable) continue;
                if (neighbor.IsObstacle) continue;
                if (neighbor.Piece != null) continue;
                if (closedTiles.Contains(neighbor)) continue;

                int tentativeG = nodes[current].g + 1;
                int g = nodes.ContainsKey(neighbor) ? nodes[neighbor].g : int.MaxValue;
                if (tentativeG >= g) continue;

                // Record the better path.
                Node newNode = new(tentativeG, GetHScore(neighbor, end));
                nodes[neighbor] = newNode;
                cameFrom[neighbor] = current;

                if (!openTiles.Contains(neighbor))
                    openTiles.Add(neighbor);
            }
        }

        return result;
    }


    private int GetHScore(HexTile tile, HexTile end)
    {
        int deltaQ = tile.AxialCoordinate.x - end.AxialCoordinate.x;
        int deltaR = tile.AxialCoordinate.y - end.AxialCoordinate.y;
        return (Mathf.Abs(deltaQ) + Mathf.Abs(deltaQ + deltaR) + Mathf.Abs(deltaR)) / 2;
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
        while (cameFrom.ContainsKey(current))
        {
            totalPath.Push(current);
            current = cameFrom[current];
        }
        totalPath.Push(current);
        return totalPath;
    }
}
