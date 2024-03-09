using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawningScript : MonoBehaviour
{
    private GameObject _enemySpawnPoint;

    private GameObject _gameController;
    private List<GameObject> _enemyPools = new List<GameObject>();
    private GameObject _statusbarPool;

    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private float spawnTimer;

    private void Awake()
    {
        _enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawningPoint");
        _gameController = GameObject.FindGameObjectWithTag("GameController");

        GameObject[] enemyPools = GameObject.FindGameObjectsWithTag("EnemyPool");
        foreach (var enemyPool in enemyPools)
        {
            _enemyPools.Add(enemyPool);
        }

        _statusbarPool = GameObject.FindGameObjectWithTag("Status Bar Pool");

        spawnTimer = spawnDelay;
    }

    private void Update()
    {
        spawnTimer -= 1 * Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnTimer = spawnDelay;
            
            Spawn();
        }
    }

    private void Spawn()
    {
        int randInt = Random.Range(0, _enemyPools.Count);

        GameObject newEnemy = _enemyPools[randInt].GetComponent<ObjectPoolScript>().GetObject();
        newEnemy.transform.position = _enemySpawnPoint.transform.position + new Vector3(0, 0.75f, 0);
        BaseEnemyScript newEnemyScript = newEnemy.GetComponent<BaseEnemyScript>();

        GameObject newStatusbar = _statusbarPool.GetComponent<ObjectPoolScript>().GetObject();
        newStatusbar.GetComponent<StatusBarScript>()
            .SetUpBar(newEnemyScript.health, newEnemyScript.shield, newEnemyScript.armor);
        newStatusbar.GetComponent<StatusBarScript>().owner = newEnemy;

        newEnemyScript.statusBar = newStatusbar;
    }

    public void ReturnEnemy(GameObject enemyToReturn)
    {
        foreach (var enemyPool in _enemyPools)
        {
            enemyPool.GetComponent<ObjectPoolScript>().ReturnObject(enemyToReturn);
        }
    }
}
