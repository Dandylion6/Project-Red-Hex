using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesperateSnap : AIDecision<DesperationAttackAction>
{
    private const float SNAP_DELAY_SECONDS = 0.2f;


    public override IEnumerator ActionSequence()
    {
        Pathfinding.Result result = HexGridManager.Instance.CalculatePath(Brain.Piece.Occupying, Brain.Player.Occupying);
        List<HexTile> path = result.path;

        if (path.Count <= 1)
        {
            TurnManager.Instance.EndTurn();
            yield break;
        }

        int i = path.Count - 2; // Will always get to the player.
        HexTile tile = path[i];

        TurnManager.Instance.StartAction();
        StartCooldown();

        Brain.Piece.RotateTo(tile);
        if (Brain.Piece.MoveTo(tile, true))
        {
            AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.BossLunge);

            if (Data.Effect != null)
            {
                yield return new WaitForSeconds(SNAP_DELAY_SECONDS);

                AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.WolfAttack);

                EffectSequence sequence = Instantiate(Data.Effect);
                yield return sequence.PlaySeqeunce(Brain.Player.transform.position);
            }

            Brain.Player.TakeDamage(Data.Damage);
            yield break;
        }

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }


    public override bool IsValidAction()
    {
        int distance = Hexagon.HexDistance(Brain.Piece.Occupying, Brain.Player.Occupying);
        if (distance > Data.AttackRange) return false;

        float healthPercentage = (Brain.Piece.Health / (float)Brain.Piece.MaxHealth) * 100.0f;
        if (healthPercentage > Data.DesperationThreshold) return false;
        return true;
    }
}
