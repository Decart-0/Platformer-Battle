using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DetectorPlayer))]
[RequireComponent(typeof(GhostAnimator))]
public class Ghost : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldownAttack;

    private DetectorPlayer _detectorPlayer;
    private GhostAnimator _animator;
    private bool _isAttack;

    private void Awake()
    {
        _detectorPlayer = GetComponent<DetectorPlayer>();
        _animator = GetComponent<GhostAnimator>();   
        _isAttack = false;
    }

    private void OnEnable()
    {
        _detectorPlayer.MetedPlayer += UpdateStatusAttack;
        _detectorPlayer.MetedPlayer += Attack;
    }

    private void Update()
    {
        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        _detectorPlayer.MetedPlayer -= UpdateStatusAttack;
        _detectorPlayer.MetedPlayer -= Attack;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }

    private void UpdateStatusAttack() 
    {
        if (_detectorPlayer.NumberEnters > 1) 
        {
            _isAttack = true;
        }
        else 
        { 
            _isAttack = false;
        }
    }

    private void Attack() 
    {   
        _animator.Setup(_isAttack);
        StartCoroutine(WaitAttack());
    }

    private IEnumerator WaitAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_cooldownAttack);

        while (_isAttack)
        {
            _detectorPlayer.HealthPlayer.TakeDamage(_damage);
            
            yield return waitForSeconds;
        }
    }
}