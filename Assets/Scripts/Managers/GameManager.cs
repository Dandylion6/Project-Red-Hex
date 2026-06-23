using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] private TilePiece player = null;
    [SerializeField] private SettingsManager settingsManager = null;


    public TilePiece Player => player;
}
