using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TilePiece : MonoBehaviour
{
    [Header("Piece Settings")]
    [SerializeField] private int baseMoveDistance = 1;
    [SerializeField][Min(1)] private int maxHealth = 5;

    [Header("Animation Settings")]
    [SerializeField][Min(0.1f)] private float moveTime = 0.5f;


    public HexTile Occupying => occupying;
    public int MaxMoveDistance => Mathf.RoundToInt(baseMoveDistance + moveDistanceMultiplier);
    public bool HasTurn => hasTurn;

    private HexTile occupying = null;
    private float moveDistanceMultiplier = 1.0f;
    private int health = 0;
    private bool hasTurn = false;


    public void AddMoveMultiplier(float multiplier) => moveDistanceMultiplier += multiplier;
    public void RemoveMoveMultiplier(float multiplier) => moveDistanceMultiplier -= multiplier;

    public void Heal(int amount) => health = Mathf.Min(health + amount, maxHealth);
    
    
    public virtual void Die()
    {
        TurnManager.Instance.RemovePieceFromTurns(this);
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0) Die();
    }


    public void SpawnAt(HexTile tile)
    {
        tile.SetPiece(this);
        occupying = tile;
        transform.position = tile.transform.position;
    }


    public bool MoveTo(Vector3 target)
    {
        Vector2Int axialCoordinate = Hexagon.WorldToAxial(new(target.x, target.z));
        HexTile tile = HexGridManager.Instance.GetTile(axialCoordinate);
        return MoveTo(tile);
    }


    public bool MoveTo(HexTile tile)
    {
        if (!hasTurn) return false;
        if (!tile.CanSetPiece(this)) return false;
        if (occupying != null) 
            occupying.RemovePiece();
        
        tile.SetPiece(this);
        occupying = tile;

        Vector3 endPosition = tile.transform.position;
        TurnManager.Instance.StartAction();

        transform.DOMoveY(transform.position.y + 1.0f, moveTime * 0.4f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            transform.DOMoveY(endPosition.y, moveTime * 0.6f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                TurnManager.Instance.EndAction();
                EndTurn();
            }
            ).Play();
        }
        ).Play();

        transform.DOMoveX(endPosition.x, moveTime * 0.8f).SetEase(Ease.InOutCirc).Play();
        transform.DOMoveZ(endPosition.z, moveTime * 0.8f).SetEase(Ease.InOutCirc).Play();

        return true;
    }


    public void StartTurn() => hasTurn = true;
    public void EndTurn() => hasTurn = false;


    private void Awake()
    {
        health = maxHealth;
    }
}
