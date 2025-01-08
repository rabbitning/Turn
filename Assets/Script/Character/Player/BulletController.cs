using UnityEngine;

public class BulletController : EffectByViewChange
{
    [SerializeField] float _lifeTime = 0;
    [HideInInspector] public float Damage = 0;
    Rigidbody2D _rb;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(End), _lifeTime);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _rb.velocity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(Damage);
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Knockback(_rb.velocity.normalized, Damage * 2);
            }
        }
        End();
    }

    void End()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }
}