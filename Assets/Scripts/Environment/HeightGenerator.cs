using System.Reflection;
using UnityEditor;
using UnityEngine;

public class HeightGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private float scale = 20.0f;
    [SerializeField][Min(1)] private int octaves = 3;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private float lucranarity = 2.0f;
    [SerializeField] private float seed = 0.0f;
    [SerializeField] private bool generateWithNewSeed = true;

    public void Generate()
    {
        HexTileMapBinder binder = FindFirstObjectByType<HexTileMapBinder>();
        if (binder == null) return;

        if (generateWithNewSeed) seed = Random.value * 100000.0f;
        for (int i = 0; i < binder.AxialCoordinates.Count; ++i)
        {
            Vector2Int axialCoordinate = binder.AxialCoordinates[i];
            HexTile tile = binder.HexTiles[i];

            float noise = FractalNoise(axialCoordinate.x, axialCoordinate.y);
            float height = Mathf.Lerp(tile.HeightMinimum, tile.HeightMaximum, noise);

            Vector3 position = tile.transform.position;
            position.y = height;
            tile.transform.position = position;
        }
    }


    private float FractalNoise(float x, float y)
    {
        float value = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float maxValue = 0.0f;

        for (int i = 0; i < octaves; ++i)
        {
            float sampleX = (x + seed) / scale * frequency;
            float sampleY = (y + seed * 1.3721f) / scale * frequency;

            value += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;
            maxValue += amplitude;

            frequency *= persistence;
            amplitude *= lucranarity;
        }

        return value / maxValue;
    }


    public void Flatten()
    {
        HexTileMapBinder binder = FindFirstObjectByType<HexTileMapBinder>();
        if (binder == null) return;

        for (int i = 0; i < binder.AxialCoordinates.Count; ++i)
        {
            HexTile tile = binder.HexTiles[i];
            Vector3 position = tile.transform.position;
            position.y = 0.0f;
            tile.transform.position = position;
        }
    }
}


[CustomEditor(typeof(HeightGenerator))]
public class HeightGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10.0f);

        HeightGenerator generator = target as HeightGenerator;

        if (GUILayout.Button("Generate Height"))
            generator.Generate();

        if (GUILayout.Button("Flatten"))
            generator.Flatten();

    }
}


public class HeightGenHierarchyObject
{
    [MenuItem("Dependencies/Add Tile Height Generator", false, 10)]
    static public void CreateBinderObject(MenuCommand command)
    {
        GameObject gameObject = new("Hex Height Generator");
        gameObject.AddComponent<HeightGenerator>();

        GameObjectUtility.SetParentAndAlign(gameObject, command.context as GameObject);
        Undo.RegisterCreatedObjectUndo(gameObject, "Ceate Height Generator");

        Selection.activeObject = gameObject;
    }
}