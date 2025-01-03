using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] float _spawnCooldown = 1.5f;
    WaitForSeconds _spawnWait;
    void Start()
    {
        _spawnWait = new WaitForSeconds(_spawnCooldown);
        StartCoroutine(SpawnEnemy());
    }

    //     void Update()
    //     {

    //     }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return _spawnWait;
            int spawnIndex = Random.Range(0, _spawnPoints.Length);
            int enemyIndex = Random.Range(0, _enemyPrefabs.Length);
            Instantiate(_enemyPrefabs[enemyIndex], _spawnPoints[spawnIndex].position, Quaternion.identity);
        }
    }

}
