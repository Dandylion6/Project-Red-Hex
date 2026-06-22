using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TilePiece))]
public class MovementController : MonoBehaviour
{
    private TilePiece playerPiece = null;
    private Camera mainCamera = null;
    private Vector2 currentScreenPosition = Vector2.zero;
    private bool playerSelected = false;


    public void OnScreenInput(InputAction.CallbackContext context) => currentScreenPosition = context.ReadValue<Vector2>();


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!GetHitTile(out HexTile tile)) return;

        if (tile.Piece == GameManager.Instance.Player)
        {
            playerSelected = !playerSelected;
            transform.position += playerSelected ? Vector3.up * 0.3f : Vector3.down * 0.3f;
            return;
        }

        if (playerSelected)
        {
            playerPiece.MoveTo(tile);
            playerSelected = false;
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
}
