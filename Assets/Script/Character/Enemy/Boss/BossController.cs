using System.Collections;
using UnityEngine;

public class BossController : Character, IDamageable
{
    public enum BossPhase { Phase1, Phase2 }
    public BossPhase CurrentPhase = BossPhase.Phase1;

    [Space(10)]
    [SerializeField] GameObject _bulletPortalPrefab = null;
    [SerializeField] float _bulletPortalDistance = 5f;

    [Space(10)]
    [SerializeField] GameObject _bulletPrefab = null;
    [SerializeField] float _bulletSpeed = 20f;

    [Space(10)]
    [SerializeField] GameObject _lightningPrefab = null;
    [SerializeField] Vector2[] _lightningPosition = new Vector2[2];

    [Space(10)]
    [SerializeField] GameObject _boxWarningPrefab = null;
    [SerializeField] GameObject _circleWarningPrefab = null;

    [Space(10)]
    [SerializeField] GameObject _blockPrefab = null;

    [Space(10)]
    [SerializeField] GameObject _laserPrefab = null;

    [Space(10)]
    [SerializeField] GameObject _movingSpikePrefab = null;
    [SerializeField] float _movingSpikeSpeed = 40f;
    [SerializeField] float _movingSpikeDistance = 50f;
    [SerializeField] float _movingSpikeOffset = 25f;
    [SerializeField] Vector2 _movingSpikeSize = Vector2.zero;

    [Space(10)]
    [SerializeField] Color _afterimageColor = Color.white;
    [SerializeField] float _attackCooldown = 2f;

    SpriteRenderer _spriteRenderer = null;
    Color _originalColor = Color.white;
    Vector2 _originalPosition = Vector2.zero;
    float _attackTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    protected override void Start()
    {
        base.Start();
        _originalPosition = transform.position;
        StartCoroutine(CAfterimage());
    }

    void Update()
    {
        if (!CanMove) return;

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackCooldown)
        {
            _attackTimer = 0f;
            PerformAttack();
        }
    }

    protected override void MoveInSS()
    {
        float floatAmplitude = 3f;
        float floatSpeed = 2f;
        Vector2 newPosition = _originalPosition + new Vector2(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude);
        transform.position = newPosition;
    }

    protected override void MoveInTD()
    {
        MoveInSS();
    }

    IEnumerator CAfterimage()
    {
        while (CurrentStatsData[StatName.Health] > 0)
        {
            GameObject afterimage = Instantiate(gameObject, transform.position, transform.rotation);
            Destroy(afterimage.GetComponent<BossController>()); // Remove the BossController script from the afterimage
            Destroy(afterimage.GetComponent<Collider2D>()); // Remove any colliders from the afterimage

            if (afterimage.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.sortingOrder = -1; // Set the sorting order to be behind the original object
                spriteRenderer.color = _afterimageColor;
            }
            Destroy(afterimage, 0.4f); // Destroy the afterimage after 0.5 seconds
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void UpdateAnimationStateSS()
    {

    }

    protected override void UpdateAnimationStateTD()
    {

    }

    public void Damage(float value)
    {
        SetCurrentStatsData(StatName.Health, CurrentStatsData[StatName.Health] - value);
        if (CurrentStatsData[StatName.Health] <= 0)
        {
            if (CurrentPhase == BossPhase.Phase1)
            {
                CurrentPhase = BossPhase.Phase2;
                SetCurrentStatsData(StatName.Health, CurrentStatsData[StatName.MaxHealth]);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
        }
        else
        {
            StartCoroutine(CDamaged());
        }
    }

    IEnumerator CDamaged()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _originalColor;
    }

    void PerformAttack()
    {
        if (CurrentPhase == BossPhase.Phase1)
        {
            int attackType = Random.Range(0, 3);
            switch (attackType)
            {
                case 0:
                    // StartCoroutine(CFireBullets(5, 0.2f));
                    SummonBulletPortal(_bulletPortalDistance);
                    break;
                case 1:
                    SummonLightning(1f);
                    break;
                    // case 2:
                    // SummonBlock();
                    // break;
            }
        }
        else if (CurrentPhase == BossPhase.Phase2)
        {
            int attackType = Random.Range(0, 4);
            switch (attackType)
            {
                case 0:
                // FireLaser();
                // break;
                case 1:
                    StartCoroutine(CFireFanBullets(5, 0.3f));
                    break;
                case 2:
                // SummonBlock();
                // break;
                case 3:
                    SummonMovingSpike(2f);
                    // _attackTimer -= 1f;
                    break;
            }
        }
    }

    void SummonBulletPortal(float distanceFromPlayer)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 randomDirection;
        do
        {
            randomDirection = Random.insideUnitCircle;
        } while (Vector2.Dot(randomDirection, Vector2.up) > 0.9f);

        randomDirection = (randomDirection + Mathf.Sign(randomDirection.x) * Vector2.right).normalized;

        Vector2 spawnPosition = playerPos + randomDirection * distanceFromPlayer;

        Instantiate(_bulletPortalPrefab, spawnPosition, Quaternion.identity);
    }

    // IEnumerator CFireBullets(int amount, float fireRate)
    // {
    //     for (int i = 0; i < amount; i++)
    //     {
    //         Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
    //         GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
    //         bullet.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;

    //         yield return new WaitForSeconds(fireRate);
    //     }
    // }

    void SummonLightning(/*int amount,*/ float delay = 1f)
    {
        float xOffset = Random.Range(.5f, 4.5f);
        for (float i = xOffset; i <= _lightningPosition[1].x; i += 5)
        {
            Vector2 startPos = new(_lightningPosition[0].x + i, _lightningPosition[0].y);
            StartCoroutine(CSpawnWarningBox(startPos, delay, WarningType.Lightning, Vector2.down, Vector2.right * 1.5f));
            StartCoroutine(CSpawnLightning(startPos, delay));
        }
    }

    enum WarningType { Lightning, Laser, Block, movingSpike }
    IEnumerator CSpawnWarningBox(Vector2 position, float delay, WarningType type, Vector2 direction, Vector2 size)
    {
        GameObject warning = null;
        SpriteRenderer spriteRenderer = null;

        switch (type)
        {
            case WarningType.Lightning:
                RaycastHit2D hit = Physics2D.Raycast(position, direction, 100f, LayerMask.GetMask("Ground"));
                warning = Instantiate(_boxWarningPrefab, position - direction, Quaternion.identity);
                spriteRenderer = warning.GetComponent<SpriteRenderer>();
                size.y = hit.distance + 1f;
                spriteRenderer.size = size;
                break;
            case WarningType.Laser:
                break;
            case WarningType.Block:
                break;
            case WarningType.movingSpike:
                warning = Instantiate(_boxWarningPrefab, position, Quaternion.LookRotation(Vector3.forward, direction));
                spriteRenderer = warning.GetComponent<SpriteRenderer>();
                size.y *= -1;
                spriteRenderer.size = size;
                break;
            default:
                yield break;
        }

        Destroy(warning, delay);

        Color originalColor = spriteRenderer.color;
        float fadeDuration = delay / 2f - .1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    IEnumerator CSpawnLightning(Vector2 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(_lightningPrefab, position, Quaternion.identity);
    }

    void SummonBlock()
    {
        Vector3 warningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        GameObject warning = Instantiate(_boxWarningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnBlock), 1f);
    }

    void SpawnBlock()
    {
        Vector3 blockPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Instantiate(_blockPrefab, blockPosition, Quaternion.identity);
    }

    void FireLaser()
    {
        Vector3 warningPosition = transform.position;
        GameObject warning = Instantiate(_boxWarningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnLaser), 1f);
    }

    void SpawnLaser()
    {
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator CFireFanBullets(int amount, float fireRate)
    {
        int bulletCount = 7;
        float angleStep = 15f;
        float startAngle = -angleStep * (bulletCount - 1) / 2;

        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                float angle = baseAngle + startAngle + j * angleStep;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 20f;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    void SummonMovingSpike(float delay)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        Vector2 diraction = directions[Random.Range(0, directions.Length)];
        Vector2 startpoint = playerPos - diraction * _movingSpikeOffset;
        StartCoroutine(CSpawnWarningBox(startpoint, delay, WarningType.movingSpike, diraction, _movingSpikeSize));
        StartCoroutine(CSpawnMovingSpike(startpoint, diraction, delay));
    }

    IEnumerator CSpawnMovingSpike(Vector2 position, Vector2 diraction, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject movingSpike = Instantiate(_movingSpikePrefab, position, Quaternion.LookRotation(Vector3.forward, diraction));
        movingSpike.GetComponent<MovingSpikeController>().Init(_movingSpikeSpeed, _movingSpikeDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_lightningPosition[0], _lightningPosition[1]);
    }
}