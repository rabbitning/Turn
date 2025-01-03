using UnityEngine;

public class EnemyFish : Enemy
{
    [SerializeField] GameObject _bulletPrefab = null;
    [SerializeField] float _followRadius = 0;
    [SerializeField] float _attackRadius = 0;
    [SerializeField] float _attackCooldown = 0;

    [SerializeField] float _acceleration = 0;

    public EnemyFishIdleState IdleState { get; private set; }
    public EnemyFishFollowState FollowState { get; private set; }
    public EnemyFishAttackState AttackState { get; private set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new(this, _followRadius);
        FollowState = new(this, _followRadius, _attackRadius);
        AttackState = new(this, _attackRadius, _attackCooldown);

        StateMachine.ChangeState(IdleState);
    }

    public void Attack()
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
        bullet.GetComponent<LineRenderer>().SetPosition(1, _target.position - transform.position);
        _target.GetComponent<IDamageable>().Damage(CurrentStatsData[StatName.RangedAttackDamage]);

        Destroy(bullet, .1f);
    }

    override public void Damage(float value)
    {
        base.Damage(value);

        Animator.SetTrigger("Injury");
    }

    override protected void MoveInSS()
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

    override protected void MoveInTD()
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

        transform.localScale = new Vector3(-Mathf.Sign(_target.position.x - _rb.position.x), 1, 1);
    }

    protected override void UpdateAnimationStateTD()
    {
        Animator.SetBool("IsSS", false);

        transform.localScale = new Vector3(-Mathf.Sign(_target.position.x - _rb.position.x), 1, 1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _followRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}