using UnityEngine;

public class Coin : Loot<int>, IPickupable
{
    [field: SerializeField] public int Denomination { get; private set; }

    public override void Delete()
    {
        base.Delete();
    }
}