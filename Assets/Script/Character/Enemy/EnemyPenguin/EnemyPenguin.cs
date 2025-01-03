using UnityEngine;

public class EnemyPenguin : Enemy
{
    [SerializeField] float _followRadius = 0;
    [SerializeField] float _acceleration = 0;

    public EnemyPenguinIdleState IdleState { get; private set; }
    public EnemyPenguinFollowState FollowState { get; private set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new(this, _followRadius);
        FollowState = new(this, _followRadius);

        StateMachine.ChangeState(IdleState);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.Damage(CurrentStatsData[StatName.MeleeAttackDamage]);
        }
    }

    protected override void MoveInSS()
    {
        if (Vector2.Distance(_rb.position, _target.position) <= _followRadius)
        {
            _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, CurrentStatsData[StatName.MaxMoveSpeed] * Mathf.Sign(_target.position.x - _rb.position.x), _acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, 0, _acceleration * Time.fixedDeltaTime);
        }
        _currentVelocity.y = _rb.velocity.y;

        _rb.velocity = _currentVelocity;
    }

    protected override void MoveInTD()
    {
        // if (Vector2.Distance(_rb.position, _target.position) <= _followRadius && _path == null)
        // {
        //     InvokeRepeating(nameof(UpdatePath), 0, 0.4f);
        // }
        // else if (Vector2.Distance(_rb.position, _target.position) > _followRadius && _path != null)
        // {
        //     CancelInvoke(nameof(UpdatePath));
        //     _path = null;
        // }

        if (_path != null)
        {
            if (_currentWaypoint >= _path.vectorPath.Count) return;

            Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, direction * CurrentStatsData[StatName.MaxMoveSpeed], _acceleration * Time.fixedDeltaTime);

            _rb.velocity = _currentVelocity;

            float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
            if (distance < _nextWaypointDistance) _currentWaypoint++;
        }
        else
        {
            _rb.velocity = Vector2.MoveTowards(_rb.velocity, Vector2.zero, _acceleration * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateAnimationStateSS()
    {
        Animator.SetBool("IsSS", true);

        _spriteRenderer.flipX = !(_target.position.x - _rb.position.x < 0);
    }

    protected override void UpdateAnimationStateTD()
    {
        Animator.SetBool("IsSS", false);

        _spriteRenderer.flipX = !(_target.position.x - _rb.position.x < 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _followRadius);
    }
}