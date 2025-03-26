using System;
using System.Collections;
// using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : Character, IDamageable
{
    public static PlayerController Instance = null;
    // public Chips[] EquippedChips { get; private set; } = new Chips[3];

    #region Data Structures

    [SerializeField] ColliderData _colliderDataSS = new();
    [SerializeField] ColliderData _colliderDataTD = new();

    [SerializeField] MovementData _movementData = new();
    // Vector3 _lastPlayerPosition = Vector3.zero;

    [SerializeField] GroundCheckData _groundCheckDataSS = new();
    [SerializeField] GroundCheckData _groundCheckDataTD = new();

    [SerializeField] WallCheckData _wallCheckDataSS = new();
    [SerializeField] WallCheckData _wallCheckDataTD = new();

    [SerializeField] MeleeAttackData _meleeAttackDataSS = new();

    [SerializeField] RangedAttackData _rangedAttackDataSS = new();

    InputData _inputData = new();

    #endregion

    #region Actions

    // Action _move = null;
    Action _attack = null;
    // Action _updateAnimationState = null;

    #endregion

    #region Components References

    Rigidbody2D _rb = null;
    BoxCollider2D _col = null;
    Animator _animator = null;
    SpriteRenderer _playerSpriteRenderer = null;
    PlayerChipManager _chipManager = null;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _inputData.CanInput = true;
    }

    void Update()
    {
        HandleInput();
        _attack?.Invoke();
        _updateAnimationState?.Invoke();
        // ActiveSkill();
        CheckInvincible();
    }

    void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
        _move?.Invoke();
    }

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        _rb.velocity = _movementData.CurrentVelocity = Vector2.zero;
        ResetInput();
        if (isSS)
        {
            _col.size = _colliderDataSS.Size;
            _col.offset = _colliderDataSS.Offset;
            _move = MoveInSS;
            _attack = HandleAttackSS;
            _updateAnimationState = UpdateAnimationStateSS;
        }
        else
        {
            _col.size = _colliderDataTD.Size;
            _col.offset = _colliderDataTD.Offset;
            _move = MoveInTD;
            _attack = HandleAttackTD;
            _updateAnimationState = UpdateAnimationStateTD;
        }
    }

    // public void Respawn()
    // {
    //     transform.position = Vector3.zero;
    //     ResetInput();
    //     _inputData.CanInput = true;
    //     _rb.velocity = _movementData.CurrentVelocity = Vector2.zero;
    //     _animator.Rebind();
    //     _playerSpriteRenderer.color = Color.white;
    //     UpdateChips(EquippedChips);
    // }

    void HandleInput()
    {
        if (Time.timeScale == 0 || !_inputData.CanInput)
        {
            ResetInput();
            return;
        }

        _inputData.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump")) _inputData.JumpInput.x = true;
        if (Input.GetButtonUp("Jump")) _inputData.JumpInput.y = true;

        _inputData.AttackInput = new bool2(Input.GetMouseButton(0), Input.GetMouseButton(1));

        _inputData.MousePosInput = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.E)) _chipManager.ActiveChip(0);
        // _inputData.SkillInput[0] = true;
        if (Input.GetKey(KeyCode.Q)) _chipManager.ActiveChip(1);
        // _inputData.SkillInput[1] = true;
        if (Input.GetKey(KeyCode.C)) _chipManager.ActiveChip(2);
        // _inputData.SkillInput[2] = true;
    }

    void ResetInput()
    {
        _inputData.MoveInput = Vector2.zero;
        _inputData.JumpInput = false;
        _inputData.AttackInput = false;
        _inputData.SkillInput = new bool3();
    }

    void HandleAttackSS()
    {
        _rangedAttackDataSS.AttackCooldownTimer -= Time.deltaTime;
        _meleeAttackDataSS.AttackCooldownTimer -= Time.deltaTime;

        if (_inputData.AttackInput.y && _rangedAttackDataSS.AttackCooldownTimer <= 0)
        {
            _animator.SetTrigger("RangedAttack");

            Vector2 offset = new(_rangedAttackDataSS.ProjectileOffset.x * Mathf.Sign(_inputData.MousePosInput.x - transform.position.x), _rangedAttackDataSS.ProjectileOffset.y);
            Vector2 direction = (_inputData.MousePosInput - (_rb.position + offset)).normalized;

            GameObject projectile = Instantiate(_rangedAttackDataSS.Projectile, _rb.position + offset, quaternion.identity);

            projectile.GetComponent<BulletController>().Damage = CurrentStatsData[StatName.RangedAttackDamage];
            projectile.GetComponent<Rigidbody2D>().velocity = direction * _rangedAttackDataSS.ProjectileSpeed;

            _rangedAttackDataSS.AttackCooldownTimer = CurrentStatsData[StatName.AttackCooldown];
        }

        if (!_inputData.AttackInput.y && _inputData.AttackInput.x && _meleeAttackDataSS.AttackCooldownTimer <= 0)
        {
            _animator.SetTrigger("MeleeAttack");

            _meleeAttackDataSS.AttackCooldownTimer = CurrentStatsData[StatName.AttackCooldown];
        }
    }

    void HandleAttackTD()
    {
        _rangedAttackDataSS.AttackCooldownTimer -= Time.deltaTime;
        _meleeAttackDataSS.AttackCooldownTimer -= Time.deltaTime;

        if (_inputData.AttackInput.y && _rangedAttackDataSS.AttackCooldownTimer <= 0)
        {
            _animator.SetTrigger("RangedAttack");

            Vector2 offset = new(_rangedAttackDataSS.ProjectileOffset.x * Mathf.Sign(_inputData.MousePosInput.x - transform.position.x), _rangedAttackDataSS.ProjectileOffset.y);
            Vector2 direction = (_inputData.MousePosInput - (_rb.position + offset)).normalized;

            GameObject projectile = Instantiate(_rangedAttackDataSS.Projectile, _rb.position + offset, quaternion.identity);

            projectile.GetComponent<BulletController>().Damage = CurrentStatsData[StatName.RangedAttackDamage];
            projectile.GetComponent<Rigidbody2D>().velocity = direction * _rangedAttackDataSS.ProjectileSpeed;

            _rangedAttackDataSS.AttackCooldownTimer = CurrentStatsData[StatName.AttackCooldown];
        }

        if (!_inputData.AttackInput.y && _inputData.AttackInput.x && _meleeAttackDataSS.AttackCooldownTimer <= 0)
        {
            _animator.SetTrigger("MeleeAttack");

            _meleeAttackDataSS.AttackCooldownTimer = CurrentStatsData[StatName.AttackCooldown];
        }
    }

    void GroundCheck()
    {
        Collider2D col = Physics2D.OverlapBox(_rb.position + _groundCheckDataSS.GroundCheckOffset, _groundCheckDataSS.GroundCheckSize, 0, _groundCheckDataSS.GroundLayer);
        _groundCheckDataSS.IsGrounded = col;
        if (_groundCheckDataSS.IsGrounded && !col.CompareTag("Platform")) _groundCheckDataSS.LastGroundedPosition = transform.position;

        col = Physics2D.OverlapBox(_rb.position + _groundCheckDataTD.GroundCheckOffset, _groundCheckDataTD.GroundCheckSize, 0, _groundCheckDataTD.GroundLayer);
        _groundCheckDataTD.IsGrounded = col;
        if (_groundCheckDataTD.IsGrounded && !col.CompareTag("Platform")) _groundCheckDataTD.LastGroundedPosition = transform.position;
    }

    void WallCheck()
    {
        _wallCheckDataSS.IsBlockedByWall.x = Physics2D.OverlapBox(_rb.position + new Vector2(_wallCheckDataSS.WallCheckOffsetX.x * Mathf.Sign(_inputData.MoveInput.x), _wallCheckDataSS.WallCheckOffsetX.y), _wallCheckDataSS.WallCheckSizeX, 0, _wallCheckDataSS.WallLayer);
        _wallCheckDataTD.IsBlockedByWall.x = Physics2D.OverlapBox(_rb.position + new Vector2(_wallCheckDataTD.WallCheckOffsetX.x * Mathf.Sign(_inputData.MoveInput.x), _wallCheckDataTD.WallCheckOffsetX.y), _wallCheckDataTD.WallCheckSizeX, 0, _wallCheckDataTD.WallLayer);
        if (Mathf.Sign(_inputData.MoveInput.y) < 0)
            _wallCheckDataTD.IsBlockedByWall.y = Physics2D.OverlapBox(_rb.position, _wallCheckDataTD.WallCheckSizeY, 0, _wallCheckDataSS.WallLayer);
        else
            _wallCheckDataTD.IsBlockedByWall.y = Physics2D.OverlapBox(_rb.position + _wallCheckDataTD.WallCheckOffsetY, _wallCheckDataTD.WallCheckSizeY, 0, _wallCheckDataTD.WallLayer);
    }

    protected override void MoveInSS()
    {
        if (transform.position.y < -20 && CurrentStatsData[StatName.Invincible] == 0)
        {
            Damage(10);
            if (CurrentStatsData[StatName.Health] > 0) StartCoroutine(CResetPlayerPosition(_groundCheckDataSS.LastGroundedPosition));
        }

        _movementData.JumpBufferTimer = _inputData.JumpInput.x ? _movementData.JumpBufferTime : _movementData.JumpBufferTimer - Time.fixedDeltaTime;
        _inputData.JumpInput.x = false;
        _movementData.CoyoteTimer = _groundCheckDataSS.IsGrounded ? _movementData.CoyoteTime : _movementData.CoyoteTimer - Time.fixedDeltaTime;

        if (_movementData.JumpBufferTimer >= 0 && _movementData.CoyoteTimer >= 0)
        {
            _rb.AddForce(Vector2.up * CurrentStatsData[StatName.JumpForceMultiplier], ForceMode2D.Impulse);
            _movementData.JumpBufferTimer = -1;
            _movementData.CoyoteTimer = -1;
        }

        if (_inputData.JumpInput.y && !_groundCheckDataSS.IsGrounded && _rb.velocity.y > 0)
            _rb.AddForce(Vector2.down * CurrentStatsData[StatName.FallForceMultiplier], ForceMode2D.Force);
        else _inputData.JumpInput.y = false;

        float acceleration = _groundCheckDataSS.IsGrounded ? CurrentStatsData[StatName.GroundAcceleration] : CurrentStatsData[StatName.AirAcceleration];
        _movementData.CurrentVelocity.x = Mathf.MoveTowards(_movementData.CurrentVelocity.x, _inputData.MoveInput.x * CurrentStatsData[StatName.MaxMoveSpeed], acceleration * Time.fixedDeltaTime);

        if (_wallCheckDataSS.IsBlockedByWall.x) _movementData.CurrentVelocity.x = 0;

        _movementData.CurrentVelocity.y = _rb.velocity.y;

        _rb.velocity = _movementData.CurrentVelocity;
    }

    protected override void MoveInTD()
    {
        if (!_groundCheckDataTD.IsGrounded && CurrentStatsData[StatName.Invincible] == 0)
        {
            Damage(10);
            if (CurrentStatsData[StatName.Health] > 0) StartCoroutine(CResetPlayerPosition(_groundCheckDataTD.LastGroundedPosition));
        }

        _movementData.CurrentVelocity = Vector2.MoveTowards(_movementData.CurrentVelocity, _inputData.MoveInput.normalized * CurrentStatsData[StatName.MaxMoveSpeed], CurrentStatsData[StatName.GroundAcceleration] * Time.fixedDeltaTime);

        if (_wallCheckDataTD.IsBlockedByWall.x) _movementData.CurrentVelocity.x = 0;
        if (_wallCheckDataTD.IsBlockedByWall.y) _movementData.CurrentVelocity.y = 0;

        _rb.velocity = _movementData.CurrentVelocity;
    }

    IEnumerator CResetPlayerPosition(Vector2 newPosition)
    {
        _inputData.CanInput = false;
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(.2f);

        transform.position = newPosition;
        yield return new WaitForSeconds(.2f);

        _inputData.CanInput = true;
    }

    public void UpdateChips(Chips[] newChips)
    {


        foreach (var stat in DefaultStatsData)
        {
            //     switch (stat.Name)
            //     {
            //         // case StatName.Health:
            //         // case StatName.Shield:
            //         //     break;
            //         default:
            SetCurrentStatsData(stat.Name, stat.Value);
            //             break;
            //     }
        }
        // _inputData.SkillInput = new bool3();

        // if (EquippedChips != null)
        // {
        //     for (int i = 0; i < EquippedChips.Length; i++)
        //     {
        //         if (EquippedChips[i] == null) continue;

        //         if (EquippedChips[i] is PassiveChips passiveChip)
        //         {
        //             StopCoroutine(passiveChip.PassiveSkillCoroutine);
        //         }
        //     }
        // }

        if (newChips == null) return;
        // EquippedChips = newChips;

        foreach (var chip in newChips)
        {
            if (chip == null) continue;
            if (chip is StatChips statChip)
            {
                //     switch (chip)
                //     {
                //         case StatChips statChip:
                foreach (var statModifier in statChip.StatModifiers)
                {
                    UpdateStat(statModifier.StatData.Name, statModifier.StatData.Value, statModifier.SetType);
                }
                //             break;
                //         case PassiveChips passiveChip:
                //             passiveChip.PassiveSkillCoroutine = StartCoroutine(passiveChip.CPassiveSkill(this));
                //             break;
                //     }
            }
        }
    }

    void UpdateStat(StatName statName, float newValue, StatSetType setType)
    {
        switch (setType)
        {
            case StatSetType.無:
                return;
            case StatSetType.加算:
                SetCurrentStatsData(statName, CurrentStatsData[statName] + newValue);
                break;
            case StatSetType.乗算:
                SetCurrentStatsData(statName, CurrentStatsData[statName] * newValue);
                break;
            case StatSetType.設定:
                SetCurrentStatsData(statName, newValue);
                break;
        }
    }

    // void ActiveSkill()
    // {
    //     for (int i = 0; i < EquippedChips.Length; i++)
    //     {
    //         if (_inputData.SkillInput[i] && EquippedChips[i] is ActiveChips activeChip)
    //         {
    //             activeChip.UseActiveSkill(this);
    //             _inputData.SkillInput[i] = false;
    //         }
    //     }
    // }

    public void Damage(float value)
    {
        if (CurrentStatsData[StatName.Invincible] > 0) return;

        SetCurrentStatsData(StatName.Health, CurrentStatsData[StatName.Health] - value);
        StartCoroutine(CSetInvincible(1f));

        if (CurrentStatsData[StatName.Health] <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    IEnumerator CSetInvincible(float duration)
    {
        CurrentStatsData[StatName.Invincible] = 1;

        yield return new WaitForSeconds(duration);

        CurrentStatsData[StatName.Invincible] = 0;
    }

    void CheckInvincible()
    {
        if (CurrentStatsData[StatName.Invincible] != 0)
        {
            _playerSpriteRenderer.color = Color.red;
        }
        else
        {
            _playerSpriteRenderer.color = Color.white;
        }
    }

    protected override void UpdateAnimationStateSS()
    {
        _animator.SetBool("IsSS", true);

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _playerSpriteRenderer.flipX = _inputData.MoveInput.x != 0 ? _inputData.MoveInput.x < 0 : _playerSpriteRenderer.flipX;
        }
        else
        {
            _playerSpriteRenderer.flipX = _inputData.MousePosInput.x < transform.position.x;
        }

        _animator.SetBool("IsGrounded", _groundCheckDataSS.IsGrounded);
        _animator.SetBool("IsRunning", _groundCheckDataSS.IsGrounded && Mathf.Abs(_rb.velocity.x) > 0.1f);
        _animator.SetBool("IsJumping", !_groundCheckDataSS.IsGrounded);
        _animator.SetBool("IsLanding", _groundCheckDataSS.IsGrounded);
    }

    protected override void UpdateAnimationStateTD()
    {
        _animator.SetBool("IsSS", false);

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _playerSpriteRenderer.flipX = _inputData.MoveInput.x != 0 ? _inputData.MoveInput.x < 0 : _playerSpriteRenderer.flipX;
        }
        else
        {
            _playerSpriteRenderer.flipX = _inputData.MousePosInput.x < transform.position.x;
        }

        _animator.SetBool("IsGrounded", _groundCheckDataTD.IsGrounded);
        _animator.SetBool("IsRunning", _groundCheckDataTD.IsGrounded && _rb.velocity.magnitude > 0.1f);
        _animator.SetBool("IsFalling", !_groundCheckDataTD.IsGrounded);
    }

    void OnDrawGizmosSelected()
    {
        if (_groundCheckDataSS.ShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + _groundCheckDataSS.GroundCheckOffset, _groundCheckDataSS.GroundCheckSize);
        }
        if (_groundCheckDataTD.ShowGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + _groundCheckDataTD.GroundCheckOffset, _groundCheckDataTD.GroundCheckSize);
        }
        if (_colliderDataSS.ShowGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube((Vector2)transform.position + _colliderDataSS.Offset, _colliderDataSS.Size);
        }
        if (_colliderDataTD.ShowGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((Vector2)transform.position + _colliderDataTD.Offset, _colliderDataTD.Size);
        }
        if (_wallCheckDataSS.ShowGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((Vector2)transform.position + _wallCheckDataSS.WallCheckOffsetX, _wallCheckDataSS.WallCheckSizeX);
        }
        if (_wallCheckDataTD.ShowGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube((Vector2)transform.position + _wallCheckDataTD.WallCheckOffsetX, _wallCheckDataTD.WallCheckSizeX);
            Gizmos.DrawWireCube((Vector2)transform.position + _wallCheckDataTD.WallCheckOffsetY, _wallCheckDataTD.WallCheckSizeY);
        }
        if (_rangedAttackDataSS.ShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(_rangedAttackDataSS.ProjectileOffset.x * Mathf.Sign(_inputData.MousePosInput.x - transform.position.x), _rangedAttackDataSS.ProjectileOffset.y), 0.1f);
        }
    }
}