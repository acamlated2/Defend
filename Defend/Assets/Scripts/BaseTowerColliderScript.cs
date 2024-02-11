using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerColliderScript : MonoBehaviour
{
    private GameObject _parent;

    private void Awake()
    {
        _parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _parent.GetComponent<BaseTowerScript>().enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _parent.GetComponent<BaseTowerScript>().RemoveEnemyFromSet(other.gameObject);
        }
    }
}
