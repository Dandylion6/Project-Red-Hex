using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class HexTileMapBinder : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> axialCoordinates = new();
    [SerializeField] private List<HexTile> hexTiles = new();


    public void GenerateTileMap()
    {
        axialCoordinates.Clear();
        hexTiles.Clear();

        HexTile[] sceneHexTiles = FindObjectsByType<HexTile>(FindObjectsSortMode.None);
        for (int i = 0; i < sceneHexTiles.Length; ++i)
        {
            HexTile tile = sceneHexTiles[i];
            axialCoordinates.Add(tile.AxialCoordinate);
            hexTiles.Add(tile);
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }


    public void ClearTileMap()
    {
        axialCoordinates.Clear();
        hexTiles.Clear();
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }


    private void Start() => HexGridManager.Instance.SetTileMap(RebuildDictionary());


    private Dictionary<Vector2Int, HexTile> RebuildDictionary()
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
}


[CustomEditor(typeof(HexTileMapBinder))]
public class HexTileMapBinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HexTileMapBinder binder = target as HexTileMapBinder;
        
        if (GUILayout.Button("Bake Tiles"))
            binder.GenerateTileMap();

        if (GUILayout.Button("Clear Bake"))
            binder.ClearTileMap();

        GUILayout.Space(10.0f);
        base.OnInspectorGUI();
    }
}


public class BinderHierarchyObject
{
    [MenuItem("Dependencies/Add Tile Map Binder", false, 10)]
    static public void CreateBinderObject(MenuCommand command)
    {
        GameObject gameObject = new("Hex Tile Map Binder");
        gameObject.AddComponent<HexTileMapBinder>();

        GameObjectUtility.SetParentAndAlign(gameObject, command.context as  GameObject);
        Undo.RegisterCreatedObjectUndo(gameObject, "Create Tile Map Binder");

        Selection.activeObject = gameObject;
    }
}