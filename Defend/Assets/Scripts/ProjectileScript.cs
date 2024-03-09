using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float normalDamage = 1;
    public float siegeDamage = 1;
    public float magicDamage = 1;

    [SerializeField] private float speed = 100;

    [SerializeField] private float lifetime = 15;
    private float _lifetimeMax;

    private GameObject _gameController;

    public GameObject owner;
    public GameObject target;

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        _lifetimeMax = lifetime;
    }

    private void OnEnable()
    {
        lifetime = _lifetimeMax;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (!target.activeInHierarchy)
        {
            _gameController.transform.GetChild(0).GetComponent<ObjectPoolScript>().ReturnObject(gameObject);
        }
        
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        Vector3 velocity = dir * speed;
        transform.position += velocity * Time.deltaTime;

        lifetime -= 1 * Time.deltaTime;
        if (lifetime <= 0)
        {
            _gameController.transform.GetChild(0).GetComponent<ObjectPoolScript>().ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemyScript>().Damage(normalDamage, siegeDamage, magicDamage);
            
            _gameController.transform.GetChild(0).GetComponent<ObjectPoolScript>().ReturnObject(gameObject);
        }
    }
}
