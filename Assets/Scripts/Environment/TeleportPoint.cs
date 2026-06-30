using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexTile))]
public class TeleportPoint : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private string currentScene = "Current Scene";
    [SerializeField] private string goToScene = "Next Scene";


    private HexTile tile = null;


    private bool CanTeleport(TilePiece piece)
    {
        if (piece != GameManager.Instance.Player) return false;
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

        if (goToScene != string.Empty)
            GameManager.Instance.ChangeGameSceneAsync(goToScene);
    }
}
