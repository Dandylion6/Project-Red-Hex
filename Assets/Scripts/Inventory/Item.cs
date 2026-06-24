using UnityEngine;
[System.Serializable]
public abstract class Item : MonoBehaviour
{
    

    private TilePiece player = GameManager.Instance.Player;

    public TilePiece Player => player;
        
    public abstract void Use();
}
