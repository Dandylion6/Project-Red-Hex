public class MoonShard : PickupTile
{
    protected override void OnPickup()
    {
        if (TeleportPoint.Instance == null) return;

        TeleportPoint.Instance.AddMoonShard();
        Destroy(ToRemove);
    }
}
