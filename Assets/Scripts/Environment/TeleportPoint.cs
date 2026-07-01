using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexTile))]
public class TeleportPoint : Singleton<TeleportPoint>
{
    public const int REQUIRED_SHARDS = 3;


    [Header("Teleport Settings")]
    [SerializeField] private GameObject closedDoor = null;
    [SerializeField] private GameObject openedDoor = null;
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
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        bool isOpen = moonShardsCollected >= REQUIRED_SHARDS;

        closedDoor.SetActive(!isOpen);
        openedDoor.SetActive(isOpen);
    }


    private void MoveToNextScene(TilePiece piece)
    {
        if (!CanTeleport(piece)) return;

        if (goToScene != string.Empty)
            GameManager.Instance.ChangeGameSceneAsync(goToScene);
    }


    private void OnDestroy()
    {
        if (tile != null) tile.UnsubscribeFromOnPiecePlaced(MoveToNextScene);
        if (TurnManager.Instance != null) TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }
}
