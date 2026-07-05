using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerCamera : Singleton<PlayerCamera>
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform = null;

    [Header("Camera Settings")]
    [SerializeField] private float cameraMoveTime = 0.2f;
    [SerializeField] private float swayStrength = 0.5f;
    [SerializeField] private float swaySpeed = 0.8f;
    [SerializeField][Range(0.0f, 1.0f)] private float inCombatPlayerBias = 0.6f;

    private HexTile target = null;
    private Vector3 followVelocity = Vector3.zero;
    private Vector3 trackedPosition = Vector3.zero;
    private Vector3 seed = Vector3.zero;


    public void SetTarget(HexTile target) => this.target = target;

    public void SnapToTarget(HexTile target)
    {
        this.target = target;
        trackedPosition = target.transform.position;
        followVelocity = Vector3.zero;
    }


    void Update()
    {
        Vector3 targetPosition;
        if (TurnManager.Instance.IsInCombat) targetPosition = UpdateCombat();
        else targetPosition = UpdateFollow();

        trackedPosition = Vector3.SmoothDamp(trackedPosition, targetPosition, ref followVelocity, cameraMoveTime);

        Vector3 noise = Vector3.one;
        noise.x = Mathf.PerlinNoise(Time.time * swaySpeed, seed.x);
        noise.y = Mathf.PerlinNoise(Time.time * swaySpeed, seed.y);
        noise.z = Mathf.PerlinNoise(Time.time * swaySpeed, seed.z);
        noise *= swayStrength;

        transform.position = trackedPosition + noise;
    }


    private Vector3 UpdateFollow()
    {
        if (target == null) return trackedPosition;
        return target.transform.position;
    }


    private Vector3 UpdateCombat()
    {
        Vector3 positionSum = Vector3.zero;
        int count = 0;

        foreach(TilePiece piece in TurnManager.Instance.ActivePieces)
        {
            positionSum += piece.Occupying.transform.position;
            ++count;
        }

        Vector3 playerPosition = GameManager.Instance.Player.Occupying.transform.position;
        Vector3 centroid = count > 0 ? positionSum / count : playerPosition;

        Vector3 final = Vector3.Lerp(centroid, playerPosition, inCombatPlayerBias);
        final.y = target.transform.position.y;
        return final;
    }


    private void Start()
    {
        seed.x = Random.Range(-10000.0f, 10000.0f);
        seed.y = Random.Range(-10000.0f, 10000.0f);
        seed.z = Random.Range(-10000.0f, 10000.0f);
    }
}
