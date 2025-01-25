using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DetectorPlayer))]
[RequireComponent(typeof(GhostAnimator))]
public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private float _attackRadius;

    private DetectorPlayer _detectorPlayer;
    private GhostAnimator _animator;
    private Coroutine _attackCoroutine;

    private void Awake()
    {
        _detectorPlayer = GetComponent<DetectorPlayer>();
        _animator = GetComponent<GhostAnimator>();
    }

    private void Update()
    {      
        if (_health <= 0)
            Destroy(gameObject);

        if (_detectorPlayer.IsPlayerVisible)
            CheckAttackZone();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }

    private void CheckAttackZone()
    {
        if (Physics2D.OverlapCircle(_attackPosition.position, _attackRadius, LayerMask.GetMask("Player")))
        {   
            StartAttack();           
        }
        else
        {
            StopAttack();
        }
    }

    private void StartAttack()
    {
        _animator.Setup(true);

        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(WaitAttack());
        }
    }

    private void StopAttack()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null; 
        }

        _animator.Setup(false);
    }

    private IEnumerator WaitAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_cooldownAttack);

        while (true)
        {
            _detectorPlayer.HealthPlayer.TakeDamage(_damage);

            yield return waitForSeconds;
        }
    }
}