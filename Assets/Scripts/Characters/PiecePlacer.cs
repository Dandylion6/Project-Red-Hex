using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(TilePiece))]
public class PiecePlacer : MonoBehaviour
{
    private void PlacePiece()
    {
        TilePiece piece = GetComponent<TilePiece>();
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        HexTile tile = HexGridManager.Instance.GetTile(hexAxial);

        if (tile == null) return;

        piece.SpawnAt(tile);
        Destroy(this); // Not needed anymore.
    }


    private void Update()
    {
        if (HexTileMapBinder.Instance.IsSetup)
            PlacePiece();
    }


#if UNITY_EDITOR
    public void Rotate(bool right)
    {
        float sign = right ? 1.0f : -1.0f;
        Quaternion rotation = transform.rotation * Quaternion.Euler(0.0f, 60.0f * sign, 0.0f);

        transform.rotation = rotation;
        EditorSceneManager.MarkAllScenesDirty();
    }


    private void OnDrawGizmosSelected() => SnapToGrid();


    private void SnapToGrid()
    {
        Vector2Int hexAxial = Hexagon.WorldToAxial(new(transform.position.x, transform.position.z));
        Vector2 world = Hexagon.AxialToWorld(hexAxial);

        transform.position = new(world.x, transform.position.y, world.y);
        EditorSceneManager.MarkAllScenesDirty();
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(PiecePlacer))]
public class PiecePlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);

        PiecePlacer placer = target as PiecePlacer;
        if (GUILayout.Button("Rotate Left"))
        {
            placer.Rotate(false);
        }

        if (GUILayout.Button("Rotate Right"))
        {
            placer.Rotate(true);
        }
    }
}
#endif