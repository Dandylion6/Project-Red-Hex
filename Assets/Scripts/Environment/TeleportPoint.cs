using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexTile))]
public class TeleportPoint : Singleton<TeleportPoint>
{
    public const int REQUIRED_SHARDS = 3;


    [Header("Teleport Settings")]
    [SerializeField] private string currentScene = "Current Scene";
    [SerializeField] private string goToScene = "Next Scene";


    public int MoonShardsCollected => moonShardsCollected;

    private HexTile tile = null;
    private int moonShardsCollected = 0;


    public void AddMoonShard() => moonShardsCollected = Mathf.Min(moonShardsCollected + 1, 3);


    private bool CanTeleport(TilePiece piece)
    {
        if (piece != GameManager.Instance.Player) return false;
        if (moonShardsCollected < REQUIRED_SHARDS) return false;
        return true;
    }


    private void Start()
    {
        tile = GetComponent<HexTile>();
        tile.SubscribeToOnPiecePlaced(MoveToNextScene);
    }


    private void MoveToNextScene(TilePiece piece)
    {
        if (!CanTeleport(piece)) return;

        if (currentScene != string.Empty) SceneManager.UnloadSceneAsync(currentScene);
        if (goToScene != string.Empty) SceneManager.LoadScene(goToScene, LoadSceneMode.Additive);
    }
}
