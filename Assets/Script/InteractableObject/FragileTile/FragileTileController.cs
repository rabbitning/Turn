using System.Collections;
using UnityEngine;

public class FragileTileController : EffectByViewChange
{
    [SerializeField] float _breakDelay = 0.5f;
    [SerializeField] float _breakDuration = 0.5f;
    [SerializeField] float _resetDelay = 2f;
    bool _isBreaking = false;

    [SerializeField] SpriteRenderer _spriteRenderer = null;
    Color _originalColor = Color.white;
    Collider2D _col = null;
    PlatformEffector2D _effector = null;
    float _originalSurfaceArc = 0;

    void Awake()
    {
        _originalColor = _spriteRenderer.color;
        _col = GetComponent<Collider2D>();
        if (TryGetComponent(out _effector)) _originalSurfaceArc = _effector.surfaceArc;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsSS) return;
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.01f)
        {
            StartCoroutine(BreakTile());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsSS) return;
        if (other.CompareTag("Player"))
        {
            StartCoroutine(BreakTile());
        }
    }

    protected override void OnSS()
    {
        base.OnSS();
        if (_col.isTrigger) _col.enabled = false;
        else if (_effector != null) _effector.surfaceArc = _originalSurfaceArc;
    }

    protected override void OnTD()
    {
        base.OnTD();
        if (_col.isTrigger) _col.enabled = true;
        else if (_effector != null) _effector.surfaceArc = 360;
    }

    IEnumerator BreakTile()
    {
        if (_isBreaking) yield break;
        _isBreaking = true;

        Vector3 originalPosition = _spriteRenderer.transform.position;
        float shakeDuration = _breakDelay;
        float shakeMagnitude = 0.05f;
        float elapsed = 0f;
        float shakeInterval = 0.05f; // Adjust this value to control the frequency of the shake

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            if (elapsed % shakeInterval < Time.deltaTime)
            {
                float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
                float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
                _spriteRenderer.transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);
            }
            yield return null;
        }

        _spriteRenderer.transform.position = originalPosition;

        float timer = 0f;
        while (timer < _breakDuration)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(_originalColor.a, 0.2f, timer / _breakDuration));
            yield return null;
        }
        _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.2f);
        _col.enabled = false;

        yield return new WaitForSeconds(_resetDelay);

        timer = 0f;
        while (timer < _breakDuration)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(_spriteRenderer.color.a, _originalColor.a, timer / _breakDuration));
            yield return null;
        }
        _spriteRenderer.color = _originalColor;
        // if (_col.isTrigger && IsSS) _col.enabled = false;
        // else
        _col.enabled = true;

        _isBreaking = false;
    }
}