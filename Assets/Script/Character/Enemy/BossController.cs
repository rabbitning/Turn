using System.Collections;
using UnityEngine;

public class BossController : Character, IDamageable
{
    public enum BossPhase { Phase1, Phase2 }
    public BossPhase currentPhase = BossPhase.Phase1;

    public GameObject bulletPrefab;
    public GameObject lightningPrefab;
    public GameObject warningPrefab;
    public GameObject blockPrefab;
    public GameObject laserPrefab;

    private float attackCooldown = 2f;
    private float attackTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            PerformAttack();
        }
    }

    protected override void MoveInSS()
    {

    }

    protected override void MoveInTD()
    {

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
        if (currentPhase == BossPhase.Phase1)
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
        else if (currentPhase == BossPhase.Phase2)
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
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 20f;
        }
    }

    void SummonLightning()
    {
        Vector3 warningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnLightning), 1f);
    }

    void SpawnLightning()
    {
        Vector3 lightningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Instantiate(lightningPrefab, lightningPosition, Quaternion.identity);
    }

    void SummonBlock()
    {
        Vector3 warningPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnBlock), 1f);
    }

    void SpawnBlock()
    {
        Vector3 blockPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Instantiate(blockPrefab, blockPosition, Quaternion.identity);
    }

    void FireLaser()
    {
        Vector3 warningPosition = transform.position;
        GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, 1f);

        Invoke(nameof(SpawnLaser), 1f);
    }

    void SpawnLaser()
    {
        Instantiate(laserPrefab, transform.position, Quaternion.identity);
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
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 20f;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}