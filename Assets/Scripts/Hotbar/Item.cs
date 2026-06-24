using Unity.Hierarchy;
using UnityEngine;
[System.Serializable]
public abstract class Item : MonoBehaviour
{


    private TilePiece player;

    private TileSelect tileSelect;

    
   
    protected TilePiece Player => player;
    protected TileSelect TileSelect => tileSelect;

    public void Start()
    {
        player = GameManager.Instance.Player;
        tileSelect = player.GetComponent<TileSelect>();
    }
    public abstract void Use();


    private void Start() => player = GameManager.Instance.Player;
}
