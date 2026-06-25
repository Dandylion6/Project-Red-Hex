using DG.Tweening;
using UnityEngine;

public class TilePiece : MonoBehaviour
{
    [Header("Piece Settings")]
    [SerializeField] private int baseMoveDistance = 1;
    [SerializeField][Min(1)] private int maxHealth = 5;

    [Header("Animation Settings")]
    [SerializeField][Min(0.1f)] private float moveTime = 0.5f;
    [SerializeField] private float moveHeight = 1.4f;
    [SerializeField] private AnimationCurve heightUp = new();
    [SerializeField] private AnimationCurve heightDown = new();


    public HexTile Occupying => occupying;
    public int MaxMoveDistance => Mathf.RoundToInt(baseMoveDistance * moveDistanceMultiplier);

    private HexTile occupying = null;
    private float moveDistanceMultiplier = 1.0f;
    private int health = 0;


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
        if (!tile.CanSetPiece(this)) return false;
        if (occupying != null) 
            occupying.RemovePiece();

        tile.SetPiece(this);
        occupying = tile;

        Vector3 endPosition = tile.transform.position;
        TurnManager.Instance.StartAction();

        transform.DOMoveY(transform.position.y + moveHeight, moveTime * 0.5f).SetEase(heightUp).OnComplete(() =>
        {
            transform.DOMoveY(endPosition.y, moveTime * 0.5f).SetEase(heightDown)
                .OnComplete(() =>
                {
                    TurnManager.Instance.EndAction();
                    TurnManager.Instance.EndTurn();
                }).Play();
        }
        ).Play();

        transform.DOMoveX(endPosition.x, moveTime).SetEase(Ease.InOutCubic).Play();
        transform.DOMoveZ(endPosition.z, moveTime).SetEase(Ease.InOutCubic).Play();

        return true;
    }


    private void Awake()
    {
        health = maxHealth;
    }
}
