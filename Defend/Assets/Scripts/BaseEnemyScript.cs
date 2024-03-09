using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour
{
    public enum Type
    {
        Normal, 
        Shielded, 
        Armored
    }

    public Type type = Type.Normal;
    [SerializeField] protected float health = 5;
    private protected float maxHealth;
    public float damage = 1;

    private GameObject _gameController;

    private void OnEnable()
    {
        GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, 0, 0));
        
        health = maxHealth;
    }

    protected virtual void Awake()
    {
        maxHealth = health;

        _gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, 0, 0));
        }
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            _gameController.transform.GetChild(1).GetComponent<ObjectPoolScript>().ReturnObject(gameObject);
            
            _gameController.GetComponent<TowerManagerScript>().RemoveEnemyFromTower(gameObject);
        }
    }
}
