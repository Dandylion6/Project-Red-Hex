using System.Collections;
using UnityEngine;

public class HealingItem : ConsumableItem<HealingItemData>
{
    protected override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();
        UseConsumable();

        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.PlayerHeal, 1.0f, true, Player.transform.position);
        GameManager.Instance.Player.Heal(Data.getHealBy);

        yield return TurnManager.TurnWait;
        TurnManager.Instance.EndTurn();
    }


    protected override void OnTileSelect(HexTile tile)
    {
        //additionally checks that player is less than max health
        if (!CanConsume(tile)) return;
        if (Player.Health >= Player.MaxHealth) return;

        StartCoroutine(ActionSequence());
    }
}