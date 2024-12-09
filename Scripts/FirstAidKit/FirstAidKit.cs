using UnityEngine;

public class FirstAidKit : Loot<float>, IPickupable
{
    [field: SerializeField] public float RestoresHealth { get; private set; }

    public override void Delete()
    {
        base.Delete();
    }
}