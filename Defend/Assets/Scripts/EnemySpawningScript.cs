using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawningScript : MonoBehaviour
{
    private GameObject[] _enemySpawnPoints;

    private GameObject _gameController;
    private List<GameObject> _enemyPools = new List<GameObject>();
    private GameObject _statusbarPool;

    [SerializeField] private float spawnDelay = 3;
    [SerializeField] private float spawnTimer;

    private void Awake()
    {
        _enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawningPoint");
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

            for (int i = 0; i < _enemySpawnPoints.Length; i++)
            {
                Spawn(_enemySpawnPoints[i]);
            }
        }
    }

    private void Spawn(GameObject spawnPoint)
    {
        int randInt = Random.Range(0, _enemyPools.Count);

        GameObject newEnemy = _enemyPools[randInt].GetComponent<ObjectPoolScript>().GetObject();
        newEnemy.GetComponent<NavMeshAgent>().Warp(spawnPoint.transform.position + new Vector3(0, 0.75f, 0));
        // newEnemy.GetComponent<NavMeshAgent>().enabled = true;
        // newEnemy.GetComponent<NavMeshAgent>().ResetPath();
        newEnemy.GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, 0, 0));
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
