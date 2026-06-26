using UnityEngine;

[RequireComponent(typeof(HexTile))]
public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        HexTile tile = GetComponent<HexTile>();
        TilePiece player = GameManager.Instance.Player;

        player.SpawnAt(tile);
        PlayerCamera.Instance.SnapToTarget(tile);
    }
}
