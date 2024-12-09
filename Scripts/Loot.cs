using UnityEngine;

public abstract class Loot<T> : MonoBehaviour
{
    public  T Value { get; private set; }

    public virtual void Delete()
    {
        Destroy(gameObject);
    }
}