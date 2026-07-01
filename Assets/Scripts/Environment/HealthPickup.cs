using UnityEngine;

public class HealthPickup : TilePiece
{
    private TilePiece player;


    private void Start()
    {
        player = GameManager.Instance.Player;
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }

    private bool PlayerDetected()
    {
        int hexDistance = Hexagon.HexDistance(player.Occupying, Occupying);
        if (hexDistance == 0) return true;
        return false;
    }

    private void PickUp()
    {
        
        
        
        base.Die();
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }

    private protected void OnTurnChanged(TilePiece peice, TilePiece lastPiece)
    {
        if (PlayerDetected())
        {
            PickUp();
        }
    }

}
