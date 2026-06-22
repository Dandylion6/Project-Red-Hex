using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] private TilePiece player = null;


    public TilePiece Player => player;
}
