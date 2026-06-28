using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthBarsParent = null;
    [SerializeField] private HealthBarUI healthBarPrefab = null;


    private readonly Dictionary<TilePiece, HealthBarUI> healthBars = new();


    private void Start()
    {
        // Player will automatically have a health bar.
        AddHealthBar(GameManager.Instance.Player, false);

        TurnManager.Instance.SubscribeToOnPieceAdded(AddHealthBar);
        TurnManager.Instance.SubscribeToOnPieceRemoved(RemoveHealthBar);
    }


    private void AddHealthBar(TilePiece piece) => AddHealthBar(piece, piece is Enemy);


    private void AddHealthBar(TilePiece piece, bool isEnemy)
    {
        if (healthBars.ContainsKey(piece))
            RemoveHealthBar(piece); // Make sure, if (somehow) there is already a health bar, to remove it.

        HealthBarUI healthBar = Instantiate(healthBarPrefab, healthBarsParent);
        healthBars.Add(piece, healthBar);
        healthBar.Initialize(piece, isEnemy);
    }


    private void RemoveHealthBar(TilePiece piece)
    {
        if (!healthBars.ContainsKey(piece)) return;
        
        HealthBarUI healthBar = healthBars[piece];
        healthBars.Remove(piece);
        Destroy(healthBar.gameObject);
    }


    private void OnDestroy()
    {
        TurnManager.Instance.UnsubscribeFromOnPieceAdded(AddHealthBar);
        TurnManager.Instance.UnsubscribeFromOnPieceRemoved(RemoveHealthBar);
    }
}
