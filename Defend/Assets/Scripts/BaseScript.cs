using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField] private float health = 10;

    private GameObject _gameController;

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Damage(other.GetComponent<BaseEnemyScript>().damage);
            
            _gameController.GetComponent<EnemySpawningScript>().ReturnEnemy(other.gameObject);
            _gameController.GetComponent<TowerManagerScript>().RemoveEnemyFromTower(other.gameObject);
        }
    }

    private void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Base Destroyed");
        }
    }
}
