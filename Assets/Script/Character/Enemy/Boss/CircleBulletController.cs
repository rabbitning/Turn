using UnityEngine;

public class CircleBulletController : MonoBehaviour
{
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] float _damage = 1f;

    void Start()
    {
        Invoke(nameof(End), _lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.Damage(_damage);
            End();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) End();
    }

    void End()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }
}
