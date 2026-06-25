using UnityEngine;

public class Enemy : TilePiece
{
    [Header("Enemy Settings")]
    [SerializeField] private int combatRange = 4;


    protected TilePiece Player => player;

    private TilePiece player = null;
    private bool isInCombat = false;


    private void Start()
    {
        player = GameManager.Instance.Player;
    }


    private void Update()
    {
        if (!isInCombat)
            EnterCombatCheck();
    }


    private void EnterCombatCheck()
    {
        if (TurnManager.Instance.IsInAction) return; // Must wait for actions first.

        int hexDistance = Hexagon.HexDistance(player.Occupying, Occupying);
        if (hexDistance > combatRange) return;

        TurnManager.Instance.AddPieceToTurns(this);
        isInCombat = true;
    }
}
