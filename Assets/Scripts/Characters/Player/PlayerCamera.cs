using UnityEngine;

public class PlayerCamera : Singleton<PlayerCamera>
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraMoveTime = 0.2f;


    private HexTile target = null;
    private Vector3 followVelocity = Vector3.zero;


    public void SetTarget(HexTile target) => this.target = target;

    public void SnapToTarget(HexTile target)
    {
        this.target = target;
        transform.position = target.transform.position;
        followVelocity = Vector3.zero;
    }


    void Update() => UpdateFollow();


    private void UpdateFollow()
    {
        if (target == null) return;

        Vector3 targetPosition = target.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref followVelocity, cameraMoveTime);
    }
}
