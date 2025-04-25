using System.Collections;
using UnityEngine;

public class BossController : Character, IDamageable
{
    public enum BossPhase { Phase1, Phase2 }
    public BossPhase CurrentPhase = BossPhase.Phase1;
    public GameObject BulletPrefab;
    public GameObject LightningPrefab;
    public GameObject WarningPrefab;
    public GameObject BlockPrefab;
    public GameObject LaserPrefab;

    [SerializeField] Color _afterimageColor = Color.white;
    Vector2 _originalPosition = Vector2.zero;
    float _attackCooldown = 2f;
    float _attackTimer = 0f;

    protected override void Start()
    {
        base.Start();
        _originalPosition = transform.position;
        StartCoroutine(CAfterimage());
    }

    void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackCooldown)
        {
            _attackTimer = 0f;
            PerformAttack();
        }
    }

    protected override void MoveInSS()
    {
        float floatAmplitude = 5f;
        float floatSpeed = 3f;
        Vector2 newPosition = _originalPosition + new Vector2(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude);
        transform.position = newPosition;
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

    protected override void MoveInTD()
    {
        MoveInSS();
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
            // Die();
        }
    }

    void PerformAttack()
    {
        if (CurrentPhase == BossPhase.Phase1)
        {
            int attackType = Random.Range(0, 3);
            switch (attackType)
            {
                case 0:
                    FireCircularBullets();
                    break;
                case 1:
                    SummonLightning();
                    break;
                case 2:
                    SummonBlock();
                    break;
            }
        }
        else if (CurrentPhase == BossPhase.Phase2)
        {
            int attackType = Random.Range(0, 3);
            switch (attackType)
            {
                case 0:
                    FireLaser();
                    break;
                case 1:
                    StartCoroutine(FireFanBullets());
                    break;
                case 2:
                    SummonBlock();
                    break;
            }
        }
    }

    void FireCircularBullets()
    {
        int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 20f;
        }
    }

    void SummonLightning()
    {
        Vector3 warningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        GameObject warning = Instantiate(WarningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnLightning), 1f);
    }

    void SpawnLightning()
    {
        Vector3 lightningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Instantiate(LightningPrefab, lightningPosition, Quaternion.identity);
    }

    void SummonBlock()
    {
        Vector3 warningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        GameObject warning = Instantiate(WarningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnBlock), 1f);
    }

    void SpawnBlock()
    {
        Vector3 blockPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Instantiate(BlockPrefab, blockPosition, Quaternion.identity);
    }

    void FireLaser()
    {
        Vector3 warningPosition = transform.position;
        GameObject warning = Instantiate(WarningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnLaser), 1f);
    }

    void SpawnLaser()
    {
        Instantiate(LaserPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator FireFanBullets()
    {
        int bulletCount = 7;
        float angleStep = 15f;
        float startAngle = -angleStep * (bulletCount - 1) / 2;

        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                float angle = baseAngle + startAngle + j * angleStep;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 20f;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

    }
}