using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawningScript : MonoBehaviour
{
    private GameObject _enemySpawnPoint;

    private GameObject _gameController;
    private GameObject _enemyPool;

    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private float spawnTimer;

    private void Awake()
    {
        _enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawningPoint");
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        _enemyPool = _gameController.transform.GetChild(1).gameObject;

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
        GameObject newEnemy = _enemyPool.GetComponent<ObjectPoolScript>().GetObject();
        newEnemy.transform.position = _enemySpawnPoint.transform.position + new Vector3(0, 0.75f, 0);
    }
}
