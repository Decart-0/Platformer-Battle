using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DetectorPlayer))]
public class Ghost : MonoBehaviour
{
    private const float AngleLeft = 180f;

    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _waitTime;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private Transform _targetPoint;

    private DetectorPlayer _detectorPlayer;
    private Transform[] _targetPoints;
    private int _currentPointIndex;
    private bool _isWaiting;

    private void Awake()
    {
        _detectorPlayer = GetComponent<DetectorPlayer>();
        _targetPoints = new Transform[_targetPoint.childCount];
        _isWaiting = false;

        for (int i = 0; i < _targetPoints.Length; i++)
        {
            _targetPoints[i] = _targetPoint.GetChild(i);
        }
    }

    private void OnEnable()
    {
        _detectorPlayer.OnAttacked += Attake;
    }

    private void Start()
    {      
        if (_targetPoints.Length > 0)
            transform.position = _targetPoints[_currentPointIndex].position;
    }

    private void Update()
    {
        if (_targetPoints.Length > 0 && _detectorPlayer.IsAttack == false)
            Move();

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        _detectorPlayer.OnAttacked -= Attake;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }

    private void Attake() 
    {
        StartCoroutine(WaitAttack());
    }

    private void Move()
    {
        Transform targetPoint = GetNextPoint();
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        float targetX = Mathf.MoveTowards(transform.position.x, targetPoint.position.x, _speed * Time.deltaTime);
        transform.position = new Vector2(targetX, transform.position.y);

        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.1f && !_isWaiting)
        {
            StartCoroutine(WaitAtPoint());
        }

        Rotate(direction);
    }

    private void Rotate(Vector2 direction) 
    {
        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, AngleLeft, 0);
        }
    }

    private Transform GetNextPoint() 
    {
        if (_detectorPlayer.IsVisiblePlayer)
        {
            return _detectorPlayer.PositionPlayer;
        }
        else
        {
            return _targetPoints[_currentPointIndex];
        }
    }

    private IEnumerator WaitAtPoint()
    {
        _isWaiting = true;

        yield return new WaitForSeconds(_waitTime);

        _currentPointIndex = (_currentPointIndex + 1) % _targetPoints.Length;
        _isWaiting = false;
    }

    private IEnumerator WaitAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_cooldownAttack);

        while (_detectorPlayer.IsAttack)
        {
            _detectorPlayer.HealthPlayer.TakeDamage(_damage);

            yield return waitForSeconds;
        }
    }
}