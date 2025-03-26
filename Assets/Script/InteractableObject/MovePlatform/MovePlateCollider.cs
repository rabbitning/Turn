using UnityEngine;

public class MovePlateCollider : EffectByViewChange
{
    Collider2D _col = null;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        _col.enabled = !_col.isTrigger || !isSS;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsSS && collision.gameObject.TryGetComponent<CanMoveByMovePlatform>(out var movable) && collision.contacts[0].normal.y < 0)
        {
            movable.AttachToParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsSS && collision.gameObject.TryGetComponent<CanMoveByMovePlatform>(out var movable))
        {
            movable.DetachFromParent();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<CanMoveByMovePlatform>(out var movable))
        {
            movable.AttachToParent(transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<CanMoveByMovePlatform>(out var movable))
        {
            movable.DetachFromParent();
        }
    }
}
