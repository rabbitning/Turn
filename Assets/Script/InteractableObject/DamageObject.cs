using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] float _damage = 1f;

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
                player.Damage(_damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.Damage(_damage);
        }
    }
}
