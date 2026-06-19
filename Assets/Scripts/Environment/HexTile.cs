using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Hexagon Settings")]
    [SerializeField] private float size = 1.0f;


    public Vector2Int AxialCoordinate => Hexagon.WorldToAxial(new(transform.position.x, transform.position.z), size);
    public float Size => size;


    private void OnDrawGizmosSelected()
    {
        if (Application.isEditor && !Application.isPlaying) SnapToGrid();
    }


    private void SnapToGrid()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z), size);
        Vector2 world = Hexagon.AxialToWorld(hexAxial, size);
        transform.position = new(world.x, transform.position.y, world.y);
    }
}
