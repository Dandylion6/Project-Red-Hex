using System.Collections;
using UnityEngine;

public class CallingHowlAction : AIDecision<CallingHowlActionData>
{
    [Header("References")]
    [SerializeField] private TilePiece wolfPrefab = null; // Prefab for the dire wolf to spawn.


    private int callingIndex = 0; // Keeps track of which howl is being called in the sequence.
    private CallingHowlPoint currentPoint = null; // The current point in the calling howl sequence.


    public override IEnumerator ActionSequence()
    {
        currentPoint = Data.CallingHowlPoints[callingIndex++];
        StartCooldown();
        TurnManager.Instance.StartAction();

        yield return TurnManager.TurnWait;

        // Will spawn wolves next time the piece has a turn.
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
        TurnManager.Instance.EndTurn();
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        if (currentPiece != Brain.Piece) return;
        if (currentPoint == null) return;

        SpawnWolves(currentPoint.SpawnAmount);
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
    }


    private void SpawnWolves(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            HexTile spawn = GetValidSpawn();
            TilePiece wolf = Instantiate(wolfPrefab, spawn.transform);
            wolf.SpawnAt(spawn);
            wolf.RotateTo(Brain.Player.Occupying);
            TurnManager.Instance.AddPieceToTurns(wolf);
        }
    }


    private HexTile GetValidSpawn()
    {
        HexTile[] neighbors = HexGridManager.Instance.GetNeighboringTiles(Brain.Piece.Occupying);
        foreach(HexTile neighbor in neighbors)
        {
            if (neighbor == null) continue;
            if (neighbor.Piece != null) continue;
            if (neighbor.IsObstacle) continue;
            if (!neighbor.IsWalkable) continue;
            return neighbor;
        }
        return null;
    }


    public override bool IsValidAction()
    {
        if (callingIndex == Data.CallingHowlPoints.Count) return false; // All howls have been called.

        CallingHowlPoint point = Data.CallingHowlPoints[callingIndex];
        float healthPercentage = (Brain.Piece.Health / (float)Brain.Piece.MaxHealth) * 100.0f;

        if (healthPercentage > point.TriggerHealthPercentage) return false; // Current health percentage is above the trigger threshold for this howl.
        return true;
    }
}
