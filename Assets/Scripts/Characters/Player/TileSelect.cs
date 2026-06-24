using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelect : MonoBehaviour
{
    public HexTile SelectedTile => selectedTile;

    private HexTile selectedTile = null;
    private Camera mainCamera = null;
    private Vector2 currentScreenPosition = Vector2.zero;


    public void OnScreenInput(InputAction.CallbackContext context) => currentScreenPosition = context.ReadValue<Vector2>();


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!GetHitTile(out selectedTile)) return;
    }


    public void ClearSelect() => selectedTile = null;


    private bool GetHitTile(out HexTile tile)
    {
        tile = null;
        Ray ray = mainCamera.ScreenPointToRay(currentScreenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return false;
        if (!hit.collider.TryGetComponent(out tile)) return false;
        return true;
    }


    private void Start() => mainCamera = Camera.main;

    private void LateUpdate()
    {
        selectedTile = null;
    }
}
