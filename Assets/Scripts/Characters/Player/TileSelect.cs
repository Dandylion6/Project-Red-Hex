using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelect : Singleton<TileSelect>
{
    public HexTile SelectedTile => selectedTile;

    private Action<HexTile> onSelectionChanged = null;
    private Camera mainCamera = null;
    private HexTile selectedTile = null;


    public void SubscribeToOnSelectionChanged(Action<HexTile> callback) => onSelectionChanged += callback;
    public void UnsubscibeFromOnSelectionChanged(Action<HexTile> callback) => onSelectionChanged -= callback;


    public void OnInputClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;

        if (GetHitTile(screenPosition, out selectedTile))
            onSelectionChanged?.Invoke(selectedTile);
    }


    public void ClearSelect()
    {
        selectedTile = null;
        onSelectionChanged?.Invoke(null);
    }


    private bool GetHitTile(Vector2 screenPosition, out HexTile tile)
    {
        tile = null;
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return false;
        if (!hit.collider.TryGetComponent(out tile)) return false;
        return true;
    }


    private void Start() => mainCamera = Camera.main;
}
