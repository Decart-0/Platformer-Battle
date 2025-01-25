using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _value;

    public bool IsMaxValue => _value == _maxValue;

    public void Restore(float amount)
    {
        _value += amount;

        if (_value > _maxValue)
        {
            _value = _maxValue;
        }
    }

    public void TakeDamage(float damage)
    {
        _value -= damage;

        if(_value <= 0)
        {
            Destroy(gameObject);
        }
    }
}