using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapAction : AIDecision<LeapActionData>
{
    private int turnsCommitted = 0;


    public override IEnumerator ActionSequence()
    {
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(Brain.Piece.Occupying, Brain.Player.Occupying);
        List<HexTile> path = result.path;

        if (path.Count <= 1)
        {
            TurnManager.Instance.EndTurn();
            yield break;
        }

        turnsCommitted = 0; // Reset the commitment for the next time this action is used.
        StartCooldown();
        TurnManager.Instance.StartAction();

        int i = path.Count - 2; // Will get to the closest tile to the player.
        HexTile tile = path[i];

        Vector3 endPosition = tile.transform.position;
        yield return TurnManager.TurnWait;

        Brain.Piece.RotateTo(tile);
        Brain.Piece.Occupying.RemovePiece();
        Brain.Piece.SetOccupying(tile);

        Brain.Piece.transform.DOKill();
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.BossLunge, 1.0f, true, GameManager.Instance.Player.transform.position);
        Brain.Piece.transform.DOMoveY(transform.position.y + 4.0f, 0.35f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            Brain.Piece.transform.DOMoveY(endPosition.y, 0.35f).SetEase(Ease.OutBounce).OnComplete(() => Brain.Piece.MoveEnd(tile)).Play();
        }
        ).Play();

        Brain.Piece.transform.DOMoveX(endPosition.x, 0.7f).SetEase(Ease.OutQuad).Play();
        Brain.Piece.transform.DOMoveZ(endPosition.z, 0.7f).SetEase(Ease.OutQuad).Play();
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance < Data.TriggerDistance)
        {
            turnsCommitted = 0; // Reset the commitment if the player is too far away.
            return false; // There is no need to punish the player for being too safe anymore.
        }

        if (turnsCommitted < Data.TurnsToCommit)
        {
            ++turnsCommitted;
            return false; // Will just commit to the leap until possible.
        }

        return true;
    }
}
