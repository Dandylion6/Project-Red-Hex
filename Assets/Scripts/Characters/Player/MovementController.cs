using UnityEngine;

[RequireComponent(typeof(TilePiece))]
[RequireComponent(typeof(TileSelect))]
public class MovementController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float outOfCombatMoveMultiplier = 2.0f;


    private TilePiece playerPiece = null;
    private TileSelect tileSelect = null;
    private bool hasMultiplier = false;


    private void ToggleState()
    {
        if (TurnManager.Instance.PieceWithTurn != playerPiece) return;

        switch (TurnManager.Instance.CurrentState)
        {
            case TurnManager.State.None:
                {
                    transform.position += Vector3.up * 0.3f;
                    TurnManager.Instance.SetState(TurnManager.State.Move);
                    break;
                }
            case TurnManager.State.Move:
                {
                    transform.position += Vector3.down * 0.3f;
                    TurnManager.Instance.SetState(TurnManager.State.None);
                    break;
                }
            default: break;
        }
    }


    private void Start()
    {
        playerPiece = GetComponent<TilePiece>();
        tileSelect = GetComponent<TileSelect>();
    }


    private void Update()
    {
        UpdateSelection();

        bool hasChanged = hasMultiplier != TurnManager.Instance.IsInCombat;
        if (!hasChanged) return;

        if (TurnManager.Instance.IsInCombat) playerPiece.AddMoveMultiplier(outOfCombatMoveMultiplier);
        else playerPiece.RemoveMoveMultiplier(outOfCombatMoveMultiplier);
        
        hasMultiplier = TurnManager.Instance.IsInCombat;
    }


    private void UpdateSelection()
    {
        if (!TurnManager.Instance.IsPeiceWithTurn(playerPiece)) return;
        if (!tileSelect.SelectedTile) return;
        if (!tileSelect.SelectedTile.IsWalkable) return; // Don't even try to traverse.

        if (tileSelect.SelectedTile == playerPiece.Occupying)
            ToggleState();

        if (TurnManager.Instance.CurrentState != TurnManager.State.Move) return;
        if (!playerPiece.MoveTo(tileSelect.SelectedTile)) return;

        PlayerCamera.Instance.SetTarget(tileSelect.SelectedTile);
        TurnManager.Instance.SetState(TurnManager.State.None);
        playerPiece.EndTurn();
    }
}
