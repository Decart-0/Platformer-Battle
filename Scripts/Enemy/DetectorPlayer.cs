using System;
using UnityEngine;

public class DetectorPlayer : MonoBehaviour
{
    public event Action MetedPlayer;

    public int NumberEnters { get; private set; }

    public Transform PositionPlayer { get; private set; }

    public HealthPlayer HealthPlayer { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {     
        if (collider.TryGetComponent(out HealthPlayer healthPlayer))
        {
            HealthPlayer = healthPlayer;  
            PositionPlayer = healthPlayer.transform;
            NumberEnters++;
            MetedPlayer?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<HealthPlayer>())
        {
            NumberEnters--;
            MetedPlayer?.Invoke();
        }
    }
}