using UnityEngine;
[System.Serializable]
public abstract class Item : MonoBehaviour
{
    public TilePiece Player => player;

    private TilePiece player = null;

        
    public abstract void Use();


    private void Start() => player = GameManager.Instance.Player;
}
