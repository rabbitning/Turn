using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] float lifeTime = 0;
    [SerializeField] float damage = 0;

    void Start()
    {
        Invoke("End", lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damage);
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
