using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(TilePiece))]
[RequireComponent(typeof(TileSelect))]
public class MovementController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float outOfCombatMoveMultiplier = 2.0f;


    private TilePiece playerPiece = null;
    private bool hasMultiplier = false;


    private void ToggleState()
    {
        Vector3 position = playerPiece.Occupying.transform.position;
        switch (TurnManager.Instance.CurrentState)
        {
            case TurnManager.State.None:
                {
                    transform.DOMoveY(position.y + 0.3f, 0.2f).SetEase(Ease.OutBack).Play();
                    TurnManager.Instance.SetState(TurnManager.State.Move);
                    
                    AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.SelectPiece, AudioType.Sfx, 1.0f, true, transform.position);
                    HexGridManager.Instance.DisplayRange(playerPiece.Occupying, playerPiece.MaxMoveDistance);
                    break;
                }
            case TurnManager.State.Move:
                {
                    HexGridManager.Instance.ClearOverlay();
                    transform.DOMoveY(position.y, 0.2f).SetEase(Ease.InBack).OnComplete(() => playerPiece.Occupying.MakeSound()).Play();
                    TurnManager.Instance.SetState(TurnManager.State.None);
                    break;
                }
            default: break;
        }
    }

    

    private void Start()
    {
        playerPiece = GetComponent<TilePiece>();
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
    }


    private void OnTurnChanged(TilePiece withTurn, TilePiece lastTurn)
    {
        if (withTurn == lastTurn) return;

        bool playerHasTurn = withTurn == playerPiece;
        bool playerHadLastTurn = lastTurn == playerPiece;

        if (playerHasTurn) TileSelect.Instance.SubscribeToOnSelectionChanged(UpdateSelection);
        else TileSelect.Instance.UnsubscibeFromOnSelectionChanged(UpdateSelection);

    }


    private void Update()
    {
        bool outOfCombat = !TurnManager.Instance.IsInCombat;
        bool hasChanged = hasMultiplier != outOfCombat;

        if (!hasChanged) return;

        if (outOfCombat) playerPiece.AddMoveMultiplier(outOfCombatMoveMultiplier);
        else playerPiece.RemoveMoveMultiplier(outOfCombatMoveMultiplier);
        
        hasMultiplier = outOfCombat; 
    }


    private void UpdateSelection(HexTile tile)
    {
        if (!tile.IsWalkable) return; // Don't even try to traverse.
        if (tile == playerPiece.Occupying)
            ToggleState();

        if (TurnManager.Instance.CurrentState != TurnManager.State.Move) return;
        if (!tile.CanSetPiece(playerPiece)) return;

        playerPiece.RotateTo(tile);

        if (!playerPiece.MoveTo(tile)) return;

        PlayerCamera.Instance.SetTarget(tile);
        TurnManager.Instance.SetState(TurnManager.State.None);
        HexGridManager.Instance.ClearOverlay();
    }


    private void OnDestroy()
    {
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
        TileSelect.Instance.UnsubscibeFromOnSelectionChanged(UpdateSelection);
    }
}
