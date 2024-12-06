using System;
using System.Collections;
using UnityEngine;
using Color = UnityEngine.Color;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UserInput))]
[RequireComponent(typeof(DetectorGround))]
public class Player : MonoBehaviour
{
    private const float AngleRight = 0f;
    private const float AngleLeft = 180f;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce; 
    
    [Header ("Attack")]
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private float _attackRadius;
    [SerializeField] private Transform _attackPosition;

    private Rigidbody2D _rigidbody;
    private UserInput _userInput;
    private DetectorGround _detectorGround;

    private bool _isOnGround;
    private bool _isCooldown;

    public event Action OnMoved;
    public event Action OnAttacked;

    public bool IsAttack { get; private set; }

    public float CurrentMove { get; private set; }

    private void Awake()
    {
        _detectorGround = GetComponent<DetectorGround>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _userInput = GetComponent<UserInput>();
        _isOnGround = true;
        _isCooldown = false;
    }

    private void OnEnable()
    {
        _detectorGround.GroundStatusChanged += UpdateIsGround;
    }

    private void Update()
    {
        Move();
        OnMoved?.Invoke();

        if (Input.GetKeyDown(_userInput.Jump) && _isOnGround)
        {
            Jump();
        }

        if (Input.GetKeyDown(_userInput.Attack) && _isCooldown == false) 
        {
            StartAttack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
    }

    private void OnDisable()
    {
        _detectorGround.GroundStatusChanged -= UpdateIsGround;
    }

    public void UpdateIsGround()
    {
        _isOnGround = _detectorGround.IsOnGround;
    }

    private void Attack() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPosition.position, _attackRadius);

        foreach (Collider2D collider in colliders) 
        {
            if (collider.TryGetComponent(out Ghost ghost)) 
            {
                ghost.TakeDamage(_damage);
            }  
        }
    }

    private void StartAttack()
    {   
        _isCooldown = true;
        IsAttack = true;
        OnAttacked?.Invoke();
        Attack();
        StartCoroutine(WaitAttack());
    }

    private void Move()
    {
        CurrentMove = Input.GetAxis(_userInput.AxisHorizontal);

        if (CurrentMove != 0) 
        {
            _rigidbody.velocity = new Vector2(CurrentMove * _speed, _rigidbody.velocity.y);
            float angle = CurrentMove > 0 ? AngleRight : AngleLeft;
            Quaternion targetRotation = Quaternion.Euler(new Vector2(0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _jumpForce);
        }       
    }

    private void Jump() 
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(_cooldownAttack);
        
        _isCooldown = false;
        IsAttack = false;
        OnAttacked?.Invoke();  
    }
}