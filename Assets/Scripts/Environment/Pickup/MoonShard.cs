public class MoonShard : PickupTile
{
    protected override void OnPickup()
    {
        TeleportPoint.Instance.AddMoonShard();
        Destroy(ToRemove);
    }
}
