using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Hexagon Settings")]
    [SerializeField] private float size = 1.0f;


    public Vector2Int AxialCoordinate => new(hexagon.q, hexagon.r);

    private Hexagon hexagon = new();


    private void Start()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z), size);
        hexagon = new(hexAxial.x, hexAxial.y, size);
    }


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
