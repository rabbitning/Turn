using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] float _damage = 0;
    SpriteRenderer _playerSprite;

    void OnEnable()
    {
        if (PlayerController.Instance == null) return;
        if (_playerSprite == null) _playerSprite = PlayerController.Instance.GetComponent<SpriteRenderer>();

        _damage = PlayerController.Instance.CurrentStatsData[StatName.MeleeAttackDamage];
        transform.localScale = new Vector3(_playerSprite.flipX ? -1 : 1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(_damage);
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Knockback(transform.localScale.x * Vector2.right, _damage * 2);
            }
        }
    }
}
