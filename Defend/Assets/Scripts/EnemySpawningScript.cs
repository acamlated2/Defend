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
        Debug.Log(randInt);
        GameObject newEnemy = _enemyPools[randInt].GetComponent<ObjectPoolScript>().GetObject();
        newEnemy.transform.position = _enemySpawnPoint.transform.position + new Vector3(0, 0.75f, 0);
    }
}
