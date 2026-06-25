using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelect : MonoBehaviour
{
    public HexTile SelectedTile => selectedTile;

    private HexTile selectedTile = null;
    private Camera mainCamera = null;


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;
        if (!GetHitTile(screenPosition, out selectedTile)) return;
    }


    public void ClearSelect() => selectedTile = null;


    private bool GetHitTile(Vector2 screenPosition, out HexTile tile)
    {
        tile = null;
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

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
