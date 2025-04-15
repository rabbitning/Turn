using System.Collections;
using UnityEngine;

public class FragileTileController : MonoBehaviour
{
    [SerializeField] float _breakDelay = 0.5f;
    [SerializeField] float _breakDuration = 0.5f;
    [SerializeField] float _resetDelay = 2f;
    bool _isBreaking = false;

    SpriteRenderer _spriteRenderer = null;
    Color _originalColor = Color.white;
    Collider2D _col = null;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            StartCoroutine(BreakTile());
        }
    }

    IEnumerator BreakTile()
    {
        if (_isBreaking) yield break;
        _isBreaking = true;

        yield return new WaitForSeconds(_breakDelay);

        float timer = 0f;
        while (timer < _breakDuration)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(1, 0.2f, timer / _breakDuration));
            yield return null;
        }
        _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.2f);
        _col.enabled = false;

        yield return new WaitForSeconds(_resetDelay);

        timer = 0f;
        while (timer < _breakDuration)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(0.2f, 1, timer / _breakDuration));
            yield return null;
        }
        _spriteRenderer.color = _originalColor;
        _col.enabled = true;

        _isBreaking = false;
    }
}