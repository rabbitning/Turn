using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] bool _onlyUp = false;
    [SerializeField] float _damage = 1f;
    [SerializeField] bool _isResetPos = false;

    void OnCollisionStay2D(Collision2D other)
    {
        if (_onlyUp && other.GetContact(0).normal.y >= -0.01f) return; // 只在上方碰撞
        
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
