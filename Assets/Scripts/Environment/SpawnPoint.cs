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


    private void Awake()
    {
        tile = GetComponent<HexTile>();
        TilePiece player = GameManager.Instance.Player;

        tile.SubscribeToOnPiecePlaced(MoveToNextScene);

        // Will move the player to this tile.
        if (lastScene == goToScene)
            player.MoveTo(tile);
    }


    private void MoveToNextScene(TilePiece piece)
    {
        // Only check for player
        if (piece != GameManager.Instance.Player) return;

        SceneManager.UnloadSceneAsync(currentScene);
        lastScene = currentScene;
        SceneManager.LoadScene(goToScene, LoadSceneMode.Additive);
    }
}
