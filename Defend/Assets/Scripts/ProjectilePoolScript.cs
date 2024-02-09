using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolScript : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> _pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.SetActive(false);
            _pool.Add(newProjectile);
            newProjectile.transform.parent = transform;
        }
    }

    public GameObject GetProjectile()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        GameObject newProjectile = Instantiate(projectilePrefab);
        newProjectile.SetActive(true);
        _pool.Add(newProjectile);
        newProjectile.transform.parent = transform;
        return newProjectile;
    }

    public void returnProjectile(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
    }
}
