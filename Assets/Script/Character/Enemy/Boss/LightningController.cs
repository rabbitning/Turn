using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab = null;

    SpriteRenderer _spriteRenderer = null;
    CapsuleCollider2D _col = null;

    Vector2 _explosionPosition = Vector2.zero;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _col = GetComponent<CapsuleCollider2D>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));

        transform.Translate(Vector2.up);

        _spriteRenderer.flipX = Random.Range(0, 2) == 0;
        _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, hit.distance + 1f);
        _col.size = new Vector2(_col.size.x, hit.distance + 1f);
        _col.offset = new Vector2(0, -hit.distance / 2);

        _explosionPosition = hit.point;
    }

    public void Explode()
    {
        Instantiate(_explosionPrefab, _explosionPosition, Quaternion.identity);
    }
}