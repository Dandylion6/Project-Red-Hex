using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexTile))]
public class SpawnPoint : MonoBehaviour
{
    static private string lastScene = string.Empty;

    [Header("Spawn Settings")]
    [SerializeField] private string currentScene = "Current Scene";
    [SerializeField] private string goToScene = "Next Scene";

    private HexTile tile = null;


    private void Start()
    {
        tile = GetComponent<HexTile>();
        TilePiece player = GameManager.Instance.Player;

        tile.SubscribeToOnPiecePlaced(MoveToNextScene);

        // Will move the player to this tile.
        if (lastScene == goToScene)
            player.SpawnAt(tile);
    }


    private void MoveToNextScene(TilePiece piece)
    {
        // Only check for player
        if (piece != GameManager.Instance.Player) return;

        if (currentScene != string.Empty) SceneManager.UnloadSceneAsync(currentScene);
        lastScene = currentScene;
        if (goToScene != string.Empty) SceneManager.LoadScene(goToScene, LoadSceneMode.Additive);
    }
}
