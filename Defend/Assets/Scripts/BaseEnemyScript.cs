using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour
{
    [SerializeField] protected float health = 5;
    
    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, 0, 0));
        }
    }

    public bool Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
