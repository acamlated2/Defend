using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damage = 1;

    [SerializeField] private float speed = 100;

    [SerializeField] private float lifetime = 15;
    private float _lifetimeMax;

    private GameObject _gameController;

    public GameObject owner;

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
        Vector3 velocity = transform.forward * speed;
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
            other.GetComponent<BaseEnemyScript>().Damage(damage);
            
            _gameController.transform.GetChild(0).GetComponent<ObjectPoolScript>().ReturnObject(gameObject);
        }
    }
}
