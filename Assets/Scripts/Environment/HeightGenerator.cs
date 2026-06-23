using UnityEditor;
using UnityEngine;

public class HeightGenerator : MonoBehaviour
{
    [SerializeField] private float scale = 20.0f;

    public void Generate()
    {
        HexTileMapBinder binder = FindFirstObjectByType<HexTileMapBinder>();
        if (binder == null) return;

        Vector2 seed = new Vector2(Random.value, Random.value) * 100000.0f;
        for (int i = 0; i < binder.AxialCoordinates.Count; ++i)
        {
            Vector2Int axialCoordinate = binder.AxialCoordinates[i];
            HexTile tile = binder.HexTiles[i];

            float noise = Mathf.PerlinNoise((axialCoordinate.x + seed.x) * scale, (axialCoordinate.y + seed.y) * scale);
            float height = Mathf.Lerp(tile.HeightMinimum, tile.HeightMaximum, noise);

            Vector3 position = tile.transform.position;
            position.y = height;
            tile.transform.position = position;
        }
    }
}


[CustomEditor(typeof(HeightGenerator))]
public class HeightGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeightGenerator generator = target as HeightGenerator;

        if (GUILayout.Button("Generate Height"))
            generator.Generate();

        GUILayout.Space(10.0f);
        base.OnInspectorGUI();
    }
}