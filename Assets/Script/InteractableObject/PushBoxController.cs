using System;
using System.Collections;
using UnityEngine;

public class PushBoxController : EffectByViewChange
{
    [Serializable]
    struct PushPoint
    {
        public Vector2 offset;
        public Vector2 size;
        public Vector2 dir;
    }
    [SerializeField] PushPoint[] _pushPoints = new PushPoint[4];

    float _gridSize = 1f;
    [SerializeField] float _moveSpeed = 1f;
    bool _moving = false;

    Rigidbody2D _rb = null;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Move();
        }
    }

    void Move()
    {
        if (_moving) return;
        foreach (PushPoint point in _pushPoints)
        {
            Collider2D hitP = Physics2D.OverlapBox((Vector2)transform.position + point.offset, point.size, 0, LayerMask.GetMask("Player"));
            if (hitP == null) continue;

            // if (Vector2.Dot(hitP.attachedRigidbody.velocity, point.dir) >= 0f)
            // {
            Vector2 targetPos = (Vector2)transform.position + point.dir * _gridSize;
            Collider2D hitG = Physics2D.OverlapBox(targetPos, Vector2.one * .1f, 0, LayerMask.GetMask("Ground"));
            if (hitG != null) continue;

            StartCoroutine(MoveTo(targetPos));
            break;
            // }
        }
    }

    IEnumerator MoveTo(Vector2 targetPos)
    {
        _moving = true;
        if (GameManager.Instance.GetView())
        {
            while (Mathf.Abs(transform.position.x - targetPos.x) > 0.1f)
            {
                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetPos.x, Time.fixedDeltaTime * _moveSpeed), transform.position.y, transform.position.z);
                yield return null;
            }
            transform.position = new Vector2(targetPos.x, transform.position.y);
        }
        else
        {
            while (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.fixedDeltaTime * _moveSpeed);
                yield return null;
            }
            transform.position = targetPos;
        }
        _moving = false;
    }

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        if (_rb != null)
        {
            _rb.constraints = isSS ? RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX : RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _pushPoints.Length; i++)
        {
            var point = _pushPoints[i];
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + point.offset, point.size);
            Gizmos.color = Color.green;
            Gizmos.DrawLine((Vector2)transform.position + point.offset, (Vector2)transform.position + point.offset + point.dir * .2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube((Vector2)transform.position + point.dir * _gridSize, Vector3.one * .1f);
        }
    }
}