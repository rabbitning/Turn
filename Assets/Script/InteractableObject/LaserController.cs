using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] GameObject _laser = null;
    SpriteRenderer _laserSpriteRenderer = null;
    CapsuleCollider2D _laserCollider = null;
    Animator _laserAnimator = null;

    [SerializeField] bool _isLaserOn = false;
    [SerializeField] float _laserOnDuration = 5f;
    [SerializeField] float _laserOffDuration = 5f;
    float _timer = 0f;

    void Start()
    {
        _laserSpriteRenderer = _laser.GetComponent<SpriteRenderer>();
        _laserCollider = _laser.GetComponent<CapsuleCollider2D>();
        _laserAnimator = _laser.GetComponent<Animator>();

        _laserAnimator.SetBool("IsOn", _isLaserOn);
        _timer = _isLaserOn ? _laserOnDuration : _laserOffDuration;
    }
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isLaserOn = !_isLaserOn;
            _laserAnimator.SetBool("IsOn", _isLaserOn);
            _timer = _isLaserOn ? _laserOnDuration : _laserOffDuration;
        }
    }

    void FixedUpdate()
    {
        float maxDistance = 150f;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)(transform.position + transform.right * 0.6f), transform.right, maxDistance, LayerMask.GetMask("Ground"));

        float laserLength = hit.collider != null ? hit.distance : maxDistance;

        _laserSpriteRenderer.size = _laserCollider.size = new Vector2(laserLength, 1);
        _laserCollider.offset = new Vector2(laserLength / 2, _laserCollider.offset.y);
    }

    public void SetLaserOnOff()
    {
        _laser.SetActive(!_laser.activeSelf);
    }
}