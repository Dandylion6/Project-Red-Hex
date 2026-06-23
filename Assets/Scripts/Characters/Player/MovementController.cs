using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TilePiece))]
public class MovementController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float outOfCombatMoveMultiplier = 2.0f;

    private TilePiece playerPiece = null;
    private Camera mainCamera = null;
    private Vector2 currentScreenPosition = Vector2.zero;
    private bool hasMultiplier = false;


    public void OnScreenInput(InputAction.CallbackContext context) => currentScreenPosition = context.ReadValue<Vector2>();


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!GetHitTile(out HexTile tile)) return;

        if (tile.Piece == GameManager.Instance.Player)
        {
            ToggleState();
            return;
        }

        if (!tile.IsWalkable) return; // Don't even try to traverse.
        if (TurnManager.Instance.CurrentState == TurnManager.State.Move)
        {
            if (!playerPiece.MoveTo(tile)) return;
            TurnManager.Instance.SetState(TurnManager.State.None);
            playerPiece.EndTurn();
        }
    }


    private void ToggleState()
    {
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


    private bool GetHitTile(out HexTile tile)
    {
        tile = null;
        Ray ray = mainCamera.ScreenPointToRay(currentScreenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return false;
        if (!hit.collider.TryGetComponent(out tile)) return false;
        return true;
    }


    private void Start()
    {
        playerPiece = GetComponent<TilePiece>();
        mainCamera = Camera.main;
    }


    private void Update()
    {
        bool hasChanged = hasMultiplier != TurnManager.Instance.IsInCombat;
        if (!hasChanged) return;

        if (TurnManager.Instance.IsInCombat) playerPiece.AddMoveMultiplier(outOfCombatMoveMultiplier);
        else playerPiece.RemoveMoveMultiplier(outOfCombatMoveMultiplier);
        
        hasMultiplier = TurnManager.Instance.IsInCombat;
    }
}
