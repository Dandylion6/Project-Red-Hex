using UnityEngine;

public struct Hexagon
{
    public int q, r;
    public float size;

    public Hexagon(int q, int r, float size = 1.0f)
    {
        this.q = q;
        this.r = r;
        this.size = size;
    }


    static public Vector2 AxialToWorld(Vector2Int axial, float size = 1.0f) => AxialToWorld(axial.x, axial.y, size);

    static public Vector2Int WorldToAxial(Vector2 world, float size = 1.0f) => WorldToAxial(world.x, world.y, size);


    static public Vector2 AxialToWorld(int q, int r, float size = 1.0f)
    {
        float x = size * (3.0f / 2.0f * q);
        float y = size * (Mathf.Sqrt(3.0f) * (r + q / 2.0f));
        return new(x, y); 
    }


    static public Vector2Int WorldToAxial(float x, float y, float size = 1.0f)
    {
        float q = (2.0f / 3.0f * x) / size;
        float r = (-1.0f / 3.0f * x + Mathf.Sqrt(3.0f) / 3.0f * y) / size;
        return CubeRound(q, r);
    }


    public readonly Vector2Int ToCoordinate() => new(q, r);


    static public int HexDistance(HexTile tileA, HexTile tileB)
    {
        return HexDistance(tileA.AxialCoordinate, tileB.AxialCoordinate);
    }


    static public int HexDistance(Vector2 worldA, Vector2 worldB)
    {
        return HexDistance(WorldToAxial(worldA), WorldToAxial(worldB));
    }


    static public int HexDistance(Vector2Int axialA, Vector2Int axialB)
    {
        return Mathf.RoundToInt((axialA - axialB).magnitude);
    }


    /// <summary>
    /// Converts axial coordinate to the nearest hex.
    /// </summary>
    private static Vector2Int CubeRound(float q, float r)
    {
        float depth = -q - r;

        Vector3Int rounded = Vector3Int.zero;
        rounded.x = Mathf.RoundToInt(q);
        rounded.y = Mathf.RoundToInt(r);
        rounded.z = Mathf.RoundToInt(depth);

        Vector3 difference = Vector3.zero;
        difference.x = Mathf.Abs(rounded.x - q);
        difference.y = Mathf.Abs(rounded.y - r);
        difference.z = Mathf.Abs(rounded.z - depth);

        // Try to find the furthest axis to know where to round to.
        if (difference.x > difference.y && difference.x > difference.z)
            rounded.x = -rounded.y - rounded.z;
        else if (difference.y > difference.z)
            rounded.y = -rounded.x - rounded.z;

        return new(rounded.x, rounded.y);
    }
}
