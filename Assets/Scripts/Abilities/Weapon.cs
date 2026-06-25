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

        //checks if within range
        if (Hexagon.HexDistance(Player.Occupying, TileSelect.SelectedTile) <= weaponRange)
        {

            Enemy enemy = TileSelect.SelectedTile.Piece as Enemy;
            if (enemy != null)
            {
                enemy.TakeDamage(weaponDamage);
                TurnManager.Instance.EndTurn();
            }
        }
    }
}
