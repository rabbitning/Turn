using UnityEngine;
using Pathfinding;
using TMPro;
using System.Collections;

public abstract class Enemy : Character, IDamageable
{
    public StateMachine StateMachine { get; protected set; } = new();
    public Rigidbody2D _rb { get; private set; } = null;
    protected SpriteRenderer _spriteRenderer = null;
    public Animator Animator { get; private set; } = null;
    protected Vector2 _currentVelocity = Vector2.zero;
    bool _canMove = true;

    Seeker _seeker = null;
    protected Transform _target = null;
    protected Path _path = null;
    [SerializeField] protected float _nextWaypointDistance = 0;
    protected int _currentWaypoint = 0;

    [SerializeField] GameObject _damagePopupPrefab = null;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (TryGetComponent(out Animator animator)) Animator = animator;
        _seeker = GetComponent<Seeker>();
        _target = GameManager.Instance.Player.transform;
    }

    protected void Update()
    {
        StateMachine.Update();
    }

    protected void LateUpdate()
    {
        _updateAnimationState?.Invoke();
    }

    protected void FixedUpdate()
    {
        StateMachine.FixedUpdate();
        if (_canMove) _move?.Invoke();
    }

    // public void Move() => _move?.Invoke();

    // public void UpdateAnimationState() => _updateAnimationState?.Invoke();

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
    }

    public virtual void Damage(float value)
    {
        ShowDamagePopup(value);

        SetCurrentStatsData(StatName.Health, CurrentStatsData[StatName.Health] - value);
        if (CurrentStatsData[StatName.Health] <= 0)
        {
            // _currentState = EnemyState.Die;
            Destroy(gameObject);
        }
    }

    void ShowDamagePopup(float value)
    {
        GameObject damagePopup = Instantiate(_damagePopupPrefab, transform.position, Quaternion.identity);
        damagePopup.GetComponentInChildren<TextMeshProUGUI>().SetText(value.ToString());

        // Make the damage popup move upwards
        damagePopup.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1, 1), Random.Range(2, 3)) * 8;

        Destroy(damagePopup, .6f);
    }

    public virtual void Knockback(Vector2 direction, float force)
    {
        StartCoroutine(CKnockback(direction, force));
    }

    IEnumerator CKnockback(Vector2 direction, float force)
    {
        _canMove = false;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.1f);
        _canMove = true;
    }

    public void UpdatePath()
    {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, _target.position, OnPathComplete);
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    public bool IsPlayerInRadius(float radius) => Vector2.Distance(transform.position, _target.position) <= radius;
}
