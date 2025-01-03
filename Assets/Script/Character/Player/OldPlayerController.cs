using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OldPlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHp = 0;
    public float MaxHp { get => maxHp; }
    float hp = 0;
    public float Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
            if (hp == 0) { Destroy(gameObject); }
        }
    }

    [Serializable]
    struct MoveSetting
    {
        public float maxMoveSpeed;
        public float acceleration;
        // public float deceleration;
        // public float turnAcceleration;
        public float airAcceleration;
        [HideInInspector] public Vector2 currentSpeed;
        // [HideInInspector] public bool isTrun;
        // [HideInInspector] public float currentAcceleration;
        // [HideInInspector] public float currentDeceleration;
    }

    [Space(10)]

    [SerializeField] MoveSetting moveSet = new MoveSetting();
    bool isGround = false;

    [Space(10)]
    [Header("Side View Setting")]

    [SerializeField] Vector2 sideViewGroundSize = Vector2.zero;
    [SerializeField] Vector3 sideViewGroundOffset = Vector3.zero;
    [SerializeField] float jumpForceMultiplier = 0;
    [SerializeField] float cancelJumpForceMultiplier = 0;
    [SerializeField] float maxFallSpeed = 0;
    [SerializeField] LayerMask sideViewGround = 0;
    [SerializeField] float unGroundedBufferTime = 0;
    float unGroundedTimer = 0;
    [SerializeField] float passPlatformTime = 0;
    float passPlatformTimer = 0;
    [SerializeField] float preJumpBufferTime = 0;
    float preJumpTimer = 0;
    [SerializeField] List<GameObject> sideViewActiveObject = null;

    [Space(10)]
    [Header("Top View Setting")]

    [SerializeField] Vector2 topViewGroundSize = Vector2.zero;
    [SerializeField] Vector3 topViewGroundOffset = Vector3.zero;
    [SerializeField] LayerMask topViewGround = 0;
    [SerializeField] List<GameObject> topViewActiveObject = null;

    [Space(10)]
    [Header("Long Attack Setting")]

    [SerializeField] GameObject bullet = null;
    [SerializeField] float longAttackCd = 0;
    float longAttackTimer = 0;
    [SerializeField] float bulletSpeed = 0;
    [SerializeField] Vector3 bulletPosOffset = Vector3.zero;

    [Space(10)]
    [Header("Melee Attack Setting")]

    // [SerializeField] GameObject melee = null;
    [SerializeField] float meleeAttackCd = 0;
    float meleeAttackTimer = 0;
    // [SerializeField] Vector3 meleePosOffset = Vector3.zero;

    [Space(10)]
    // [Header("Other")]

    // [SerializeField] SpriteRenderer playerBodySpriteRenderer = null;
    SpriteRenderer spriteRenderer = null;
    CinemachineImpulseSource cinemachineImpulseSource = null;

    //Animator Variable
    Animator animator = null;

    //Input Variable
    Vector2 moveInput = Vector2.zero;
    bool[] jumpInput = new bool[2];
    bool longAttackInput = false;
    bool meleeAttackInput = false;
    Vector2 mousePosInput = Vector2.zero;

    Action Move = null;
    Rigidbody2D rb = null;

    void Start()
    {
        GameManager.Instance.OnViewChanged.AddListener(ViewChanged);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        hp = MaxHp;

        ViewChanged(GameManager.Instance.GetView());
    }

    void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ViewChanged(bool currentView)
    {
        rb.velocity = moveSet.currentSpeed = Vector2.zero;

        animator.SetBool("isSS", currentView);

        foreach (GameObject gameObject in sideViewActiveObject)
            gameObject.SetActive(currentView);

        foreach (GameObject gameObject in topViewActiveObject)
            gameObject.SetActive(!currentView);

        if (currentView)
        {
            Move = MoveInSideView;
        }
        else
        {
            Move = MoveInTopView;
        }
    }

    void MoveInSideView()
    {
        isGround = Physics2D.OverlapBox(transform.position + sideViewGroundOffset, sideViewGroundSize, 0, sideViewGround);
        if (isGround)
        {
            unGroundedTimer = 0;
            moveSet.currentSpeed.x = Mathf.MoveTowards(moveSet.currentSpeed.x, moveInput.x * moveSet.maxMoveSpeed, moveSet.acceleration * Time.deltaTime);

            if (moveInput.x != 0)
            {
                if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("rangeAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("meleeAttack")))
                {
                    // playerBodySpriteRenderer.flipX = moveInput.x < 0.1f;
                    spriteRenderer.flipX = moveInput.x < 0.1f;

                }
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            unGroundedTimer += Time.deltaTime;
            moveSet.currentSpeed.x = Mathf.MoveTowards(moveSet.currentSpeed.x, moveInput.x * moveSet.maxMoveSpeed, moveSet.airAcceleration * Time.deltaTime);

            animator.SetBool("isRun", false);
        }

        if (moveInput.y < -0.5f)
        {
            passPlatformTimer = 0;
            if (!Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform")))
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"));
            }
        }
        else
        {
            passPlatformTimer += Time.deltaTime;
            if (passPlatformTimer > passPlatformTime && Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform")))
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
            }
        }

        if (jumpInput[0])
        {
            preJumpTimer += Time.deltaTime;
            if (preJumpTimer > preJumpBufferTime)
            {
                jumpInput[0] = false;
            }
        }
        else
        {
            preJumpTimer = 0;
        }

        if (jumpInput[0] && unGroundedTimer <= unGroundedBufferTime)
        {
            rb.velocity = Vector2.Dot(rb.velocity, Vector2.right) * Vector2.right;
            rb.AddForce(Physics2D.gravity * -jumpForceMultiplier, ForceMode2D.Impulse);

            animator.SetTrigger("jumpTrigger");

            jumpInput[0] = false;
        }
        if (!jumpInput[1] && !isGround && rb.velocity.y > 0)
        {
            rb.AddForce(Physics2D.gravity * cancelJumpForceMultiplier, ForceMode2D.Force);
        }

        moveSet.currentSpeed.y = Mathf.Max(maxFallSpeed, rb.velocity.y);

        rb.velocity = moveSet.currentSpeed;
    }

    void MoveInTopView()
    {
        isGround = Physics2D.OverlapBox(transform.position + topViewGroundOffset, topViewGroundSize, 0, topViewGround);
        if (!isGround)
        {
            //falling out
            transform.localScale = Vector2.MoveTowards(transform.localScale, Vector2.one * 0.4f, 0.05f);
        }
        else
        {
            transform.localScale = Vector2.MoveTowards(transform.localScale, Vector2.one, 0.05f);

            if (moveInput != Vector2.zero)
            {

                if (moveInput.x != 0)
                {
                    if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("rangeAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("meleeAttack")))
                    {
                        // playerBodySpriteRenderer.flipX = moveInput.x < 0.1f;
                        spriteRenderer.flipX = moveInput.x < 0.1f;
                    }
                }
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }

        moveSet.currentSpeed = Vector2.MoveTowards(moveSet.currentSpeed, moveInput * moveSet.maxMoveSpeed, moveSet.acceleration * Time.deltaTime);
        rb.velocity = moveSet.currentSpeed;
    }

    void Attack()
    {
        if (longAttackInput && longAttackTimer >= longAttackCd)
        {
            // playerBodySpriteRenderer.flipX = mousePosInput.x - transform.position.x < 0;
            spriteRenderer.flipX = mousePosInput.x - transform.position.x < 0;

            GameObject bulletClone = Instantiate(bullet, transform.position + bulletPosOffset, Quaternion.identity);
            bulletClone.GetComponent<Rigidbody2D>().velocity = (mousePosInput - (Vector2)transform.position).normalized * bulletSpeed;

            animator.SetTrigger("rangeAtkTrigger");

            longAttackTimer = 0;
        }
        else
        {
            longAttackTimer += Time.deltaTime;
        }

        if (!longAttackInput && meleeAttackInput && meleeAttackTimer >= meleeAttackCd)
        {
            // GameObject meleeClone = Instantiate(melee, transform.position + meleePosOffset, Quaternion.identity, transform);
            // CMvcamController.cMvcamController.CameraShake(2, 0.1f);
            cinemachineImpulseSource.GenerateImpulse();

            animator.SetTrigger("meleeAtkTrigger");

            meleeAttackTimer = 0;
        }
        else
        {
            meleeAttackTimer += Time.deltaTime;
        }
    }

    public void Damage(float value)
    {
        Hp -= value;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input.normalized;
    }

    public void SetJumpInput(int index, bool input)
    {
        jumpInput[index] = input;
    }

    public void SetLongAttackInput(bool input)
    {
        longAttackInput = input;
    }

    public void SetMeleeAttackInput(bool input)
    {
        meleeAttackInput = input;
    }

    public void SetMousePosInput(Vector2 input)
    {
        mousePosInput = input;
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // if (GameManager.gameManager)
        // {
        //     if (GameManager.gameManager.GetView())
        //     {
        //         Gizmos.DrawWireCube(transform.position + sideViewGroundOffset, sideViewGroundSize);
        //     }
        //     else
        //     {
        //         Gizmos.DrawWireCube(transform.position + topViewGroundOffset, topViewGroundSize);
        //     }
        // }
        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(transform.position + bulletPosOffset, 0.05f);
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawSphere(transform.position + meleePosOffset, 0.05f);
    }
}