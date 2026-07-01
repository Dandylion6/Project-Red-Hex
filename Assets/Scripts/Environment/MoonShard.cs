using UnityEngine;

[RequireComponent(typeof(HexTile))]
public class MoonShard : MonoBehaviour
{
    [Header("References")]
    [SerializeField][Tooltip("To remove when picked up.")] private GameObject moonShard = null;


    private HexTile tile = null;


    private void Start()
    {
        if (!TryGetComponent(out tile)) return;
        tile.SubscribeToOnPiecePlaced(OnPiecePlaced);
    }


    private void OnPiecePlaced(TilePiece piece)
    {
        if (piece != GameManager.Instance.Player) return;
        if (TeleportPoint.Instance == null) return;

        Destroy(moonShard);
        TeleportPoint.Instance.AddMoonShard();
    }


    private void OnDestroy()
    {
        if (tile == null) return;
        tile.UnsubscibeToOnPiecePlaced(OnPiecePlaced);
    }
}
