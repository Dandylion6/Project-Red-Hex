using System.Collections;
using UnityEngine;

public class HealingItem : ConsumableItem<HealingItemData>
{


    protected override IEnumerator ActionSequence()
    {
        StartCooldown();
        TurnManager.Instance.StartAction();
        UseConsumable();
        yield return new WaitForSeconds(0.5f);

        //heal player
        GameManager.Instance.Player.Heal(Data.getHealBy);

        TurnManager.Instance.EndTurn();
    }


    protected override void OnTileSelect(HexTile tile)
    {
        //additionally checks that player is less than max health
        Debug.Log(Data.Count);
        if (CanConsume(tile) && GameManager.Instance.Player.Health < GameManager.Instance.Player.MaxHealth)
        
            StartCoroutine(ActionSequence());
        }

    }


