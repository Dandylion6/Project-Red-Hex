using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TilePiece))]
public class MovementController : MonoBehaviour
{
    private TilePiece playerPiece = null;
    private Camera mainCamera = null;
    private Vector2 currentScreenPosition = Vector2.zero;


    public void OnScreenInput(InputAction.CallbackContext context) => currentScreenPosition = context.ReadValue<Vector2>();


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = mainCamera.ScreenPointToRay(currentScreenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (!hit.collider.TryGetComponent(out HexTile tile)) return;

        playerPiece.MoveTo(tile);
    }


    private void Start()
    {
        playerPiece = GetComponent<TilePiece>();
        mainCamera = Camera.main;
    }
}
