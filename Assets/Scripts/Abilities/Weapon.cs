using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int weaponRange;
    [SerializeField] private int weaponDamage;


    private bool isUsing = false;

    
    public override void Use()
    {
        isUsing = true;
        HexGridManager.Instance.DisplayRange(Player.Occupying, weaponRange, HexGridManager.DisplayType.Attack);
    }


    private void Update()
    {
        if (!isUsing) return;
        if (TileSelect.SelectedTile == null) return;

        if (!HexGridManager.Instance.InLineOfSight(Player.Occupying, TileSelect.SelectedTile)) return;

        Enemy enemy = TileSelect.SelectedTile.Piece as Enemy;
        if (enemy == null) return;

        enemy.TakeDamage(weaponDamage);
        HexGridManager.Instance.ClearOverlayOfType(HexOverlay.Type.Range);
        TurnManager.Instance.EndTurn();
    }
}
