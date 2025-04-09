using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] float _damage = 1f;
    [SerializeField] bool _isResetPos = false;

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.Damage(_damage);
            if (_isResetPos) player.ResetPlayerPosition();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.Damage(_damage);
            if (_isResetPos) player.ResetPlayerPosition();
        }
    }
}
