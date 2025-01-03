using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] float _lifeTime = 0;
    [SerializeField] float _damage = 0;

    void Start()
    {
        Invoke(nameof(End), _lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(_damage);
        }
        End();
    }

    void End()
    {
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }
}
