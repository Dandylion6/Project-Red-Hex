using DG.Tweening;
using System;
using UnityEngine;

public class TilePiece : MonoBehaviour, IDamageable
{
    [Header("Piece Settings")]
    [SerializeField] private Transform character = null;
    [SerializeField] private int baseMoveDistance = 1;
    [SerializeField][Min(1)] private int maxHealth = 5;
    [SerializeField] private float healthBarHeight = 1.0f;

    [Header("Animation Settings")]
    [SerializeField][Min(0.1f)] private float moveTime = 0.5f;
    [SerializeField] private float moveHeight = 1.4f;
    [SerializeField] private AnimationCurve heightUp = new();
    [SerializeField] private AnimationCurve heightDown = new();


    public Transform Character => character;
    public HexTile Occupying => occupying;
    public int MaxMoveDistance => Mathf.RoundToInt(baseMoveDistance * moveDistanceMultiplier);
    public int MaxHealth => maxHealth;
    public int Health => health;
    public float HealthBarHeight => healthBarHeight;

    private Action<HexTile> onMove = null;
    private Action<int> onDamageTaken = null;
    private Action<int> onHeal = null;
    private HexTile occupying = null;
    private float moveDistanceMultiplier = 1.0f;
    private int health = 0;
    private bool isDead = false;


    public void AddMoveMultiplier(float multiplier) => moveDistanceMultiplier += multiplier;
    public void RemoveMoveMultiplier(float multiplier) => moveDistanceMultiplier -= multiplier;

    public void SubscribeToOnMove(Action<HexTile> callback) => onMove += callback;
    public void UnsubscribeFromOnMove(Action<HexTile> callback) => onMove -= callback;

    public void SubscribeToOnDamageTaken(Action<int> callback) => onDamageTaken += callback;
    public void UnsubscribeFromOnDamageTaken(Action<int> callback) => onDamageTaken -= callback;

    public void SubscribeToOnHeal(Action<int> callback) => onHeal += callback;
    public void UnsubscribeToOnHeal(Action<int> callback) => onHeal -= callback;


    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        onHeal?.Invoke(amount);
    }
    
    
    public virtual void Die()
    {
        isDead = true;
        transform.DOKill();
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        onDamageTaken?.Invoke(damage);

        if (isDead) return;
        if (health > 0) return;
        Die();
    }


    public void SpawnAt(HexTile tile)
    {
        if (occupying != null) occupying.RemovePiece();
        tile.SetPiece(this);

        occupying = tile;
        transform.position = tile.transform.position;
        onMove?.Invoke(tile);
    }


    public void RotateTo(HexTile tile)
    {
        if (character == null) return;

        Vector3 endPosition = tile.transform.position;
        Vector3 direction = endPosition - occupying.transform.position;
        direction.y = 0.0f;

        if (direction.magnitude <= float.Epsilon) return; // Can't turn.

        Quaternion look = Quaternion.LookRotation(direction.normalized, Vector3.up);

        character.DOKill();
        character.DORotate(look.eulerAngles, 0.4f).SetEase(Ease.OutBack).Play();
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

        occupying = tile;

        TurnManager.Instance.StartAction();
        Vector3 endPosition = tile.transform.position;

        transform.DOMoveY(transform.position.y + moveHeight, moveTime * 0.5f).SetEase(heightUp).OnComplete(() =>
        {
            transform.DOMoveY(endPosition.y, moveTime * 0.5f).SetEase(heightDown)
                .OnComplete(() =>
                {
                    tile.SetPiece(this);
                    TurnManager.Instance.EndTurn();
                    onMove?.Invoke(tile);
                }).Play();
        }
        ).Play();

        transform.DOMoveX(endPosition.x, moveTime).SetEase(Ease.OutQuad).Play();
        transform.DOMoveZ(endPosition.z, moveTime).SetEase(Ease.OutQuad).Play();

        return true;
    }


    private void Awake() => health = maxHealth;

    private void OnDestroy()
    {
        if (TurnManager.Instance == null) return;
        TurnManager.Instance.RemovePieceFromTurns(this);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position + Vector3.up * healthBarHeight, 0.1f);
    }
}
