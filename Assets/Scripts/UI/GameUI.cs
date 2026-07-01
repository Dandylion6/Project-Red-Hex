using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Quest UI Settings")]
    [SerializeField] private TMP_Text questText = null;
    [SerializeField][TextArea] private string questDescription = string.Empty;


    private void Start()
    {
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
        OnTurnChanged(null, null);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        int collected = 0;
        if (TeleportPoint.Instance != null)
        {
            collected = TeleportPoint.Instance.MoonShardsCollected;
        }
        questText.text = questDescription;
        questText.text += $"\n \n Moon Shards collected ({collected}/{TeleportPoint.REQUIRED_SHARDS})";
    }


    private void OnDestroy() => TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
}
