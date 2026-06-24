using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected TilePiece Player => player;
    protected TileSelect TileSelect => tileSelect;

    private TilePiece player;

    private TileSelect tileSelect;

    
    public abstract void Use();
    

    public void Start()
    {
        player = GameManager.Instance.Player;
        tileSelect = player.GetComponent<TileSelect>();
    }
}
