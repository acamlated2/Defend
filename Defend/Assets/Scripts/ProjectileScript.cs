using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damage = 1;

    [SerializeField] private float speed = 100;

    [SerializeField] private float lifetime = 15;

    private GameObject _gameController;

    public GameObject owner;

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Update()
    {
        Vector3 velocity = transform.forward * speed;
        transform.position += velocity * Time.deltaTime;

        lifetime -= 1 * Time.deltaTime;
        if (lifetime <= 0)
        {
            _gameController.GetComponent<ProjectilePoolScript>().returnProjectile(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _gameController.GetComponent<ProjectilePoolScript>().returnProjectile(gameObject);
            
            if (other.transform.GetComponent<BaseEnemyScript>().Damage(damage))
            {
                owner.GetComponent<BaseTowerScript>().RemoveEnemyFromList(gameObject);
            }
        }
    }
}
