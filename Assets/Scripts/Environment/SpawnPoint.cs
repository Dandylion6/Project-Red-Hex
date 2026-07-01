using UnityEngine;

[RequireComponent(typeof(HexTile))]
public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        HexTile tile = GetComponent<HexTile>();
        TilePiece player = GameManager.Instance.Player;

        GameManager.CheckpointData checkpoint = new()
        {
            playerHealth = GameManager.Instance.Player.Health
        };
        GameManager.Instance.SetCheckpointData(checkpoint);

        player.SpawnAt(tile);
        PlayerCamera.Instance.SnapToTarget(tile);
    }
}
