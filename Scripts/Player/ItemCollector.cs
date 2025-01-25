using UnityEngine;

[RequireComponent(typeof(Wallet))]
[RequireComponent(typeof(HealthPlayer))]
public class ItemCollector : MonoBehaviour
{
    private Wallet _wallet;
    private HealthPlayer _healthPlayer;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
        _healthPlayer = GetComponent<HealthPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IPickupable iPickupable))
        {
            Pickup(iPickupable);
        }
    }

    private void Pickup(IPickupable iPickupable)
    {
        switch (iPickupable) 
        {
            case FirstAidKit firstAidKit:
                if (_healthPlayer.IsMaxValue == false)
                {
                    _healthPlayer.Restore(firstAidKit.Value);
                    firstAidKit.Delete();
                }

                break;

            case Coin coin:
                _wallet.AddCoins(coin.Value);
                coin.Delete();

                break;
        }
    }
}