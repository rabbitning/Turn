using System.Collections;
using UnityEngine;

public class BulletPortalController : MonoBehaviour
{
    [SerializeField] float _delay = 0.5f;
    [SerializeField] GameObject _bulletPrefab = null;
    [SerializeField] int _bulletAmount = 10;
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _fireRate = 0.1f;

    void Start()
    {
        StartCoroutine(CFireBullets(_bulletAmount, _fireRate));
    }

    IEnumerator CFireBullets(int amount, float fireRate)
    {
        yield return new WaitForSeconds(_delay);
        
        for (int i = 0; i < amount; i++)
        {
            Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;

            yield return new WaitForSeconds(fireRate);
        }
        Destroy(gameObject, 0.5f);
    }
}
