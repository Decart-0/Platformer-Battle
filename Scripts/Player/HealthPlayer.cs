using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;

    public bool IsMaxHealth => _health == _maxHealth;

    public void RestoreHealth(float amount)
    {
        _health += amount;

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}