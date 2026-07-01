using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexTile))]
public class TeleportPoint : Singleton<TeleportPoint>
{
    [Header("Teleport Settings")]
    [SerializeField] private string currentScene = "Current Scene";
    [SerializeField] private string goToScene = "Next Scene";
    [SerializeField] private int moonShardsToCollect = 3;


    public int MoonShardsCollected => moonShardsCollected;

    private HexTile tile = null;
    private int moonShardsCollected = 0;


    public void AddMoonShard() => moonShardsCollected = Mathf.Min(moonShardsCollected + 1, moonShardsToCollect);


    private bool CanTeleport(TilePiece piece)
    {
        if (piece != GameManager.Instance.Player) return false;
        if (moonShardsCollected < moonShardsToCollect) return false;
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
