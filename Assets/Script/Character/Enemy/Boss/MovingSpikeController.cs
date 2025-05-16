using UnityEngine;

public class MovingSpikeController : EffectByViewChange
{
    float _moveSpeed = 40f;
    float _farFromPlayerDistance = 50f;
    // Vector3 _originalPosition = Vector3.zero;
    Transform _player = null;

    protected override void Start()
    {
        base.Start();
        _player = PlayerController.Instance.transform;
        // _originalPosition = transform.position;
    }
    void FixedUpdate()
    {
        transform.Translate(_moveSpeed * Time.fixedDeltaTime * Vector2.up);
        if (Vector3.Distance(transform.position, _player.position) >= 150f)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float moveSpeed, float farFromPlayerDistance)
    {
        _moveSpeed = moveSpeed;
        _farFromPlayerDistance = farFromPlayerDistance;
    }
}