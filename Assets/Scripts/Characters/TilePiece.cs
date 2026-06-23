using UnityEngine;

public class TilePiece : MonoBehaviour
{
    [Header("Piece Settings")]
    [SerializeField] private int baseMoveDistance = 1;
    [SerializeField][Min(1)] private int maxHealth = 5;


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
        transform.position = tile.transform.position;

        return true;
    }


    public void StartTurn() => hasTurn = true;
    public void EndTurn() => hasTurn = false;


    private void Awake()
    {
        health = maxHealth;
    }
}
