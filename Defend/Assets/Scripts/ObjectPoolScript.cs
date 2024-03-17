using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ObjectPoolScript : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize = 10;
    
    private List<GameObject> _pool = new List<GameObject>();
    
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            InstantiateNewObject();
        }
    }
    
    public GameObject GetObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        GameObject newObject = InstantiateNewObject();
        newObject.SetActive(true);
        poolSize = _pool.Count;

        // if (newObject.GetComponent<BaseEnemyScript>())
        // {
        //     newObject.GetComponent<NavMeshAgent>().enabled = false;
        // }
        return newObject;
    }
    
    public void ReturnObject(GameObject objectToReturn)
    {
        if (!_pool.Contains(objectToReturn))
        {
            return;
        }
        
        if (objectToReturn.tag == "Enemy")
        {
            GameObject statusBarPool = GameObject.FindGameObjectWithTag("Status Bar Pool");
            statusBarPool.GetComponent<ObjectPoolScript>().ReturnObject(objectToReturn.GetComponent<BaseEnemyScript>().statusBar);
        }
        
        objectToReturn.SetActive(false);
    }

    private GameObject InstantiateNewObject()
    {
        GameObject newObject = Instantiate(objectPrefab, transform);
        newObject.SetActive(false);
        _pool.Add(newObject);
        newObject.transform.SetParent(transform);

        if (objectPrefab.tag == "Status Bar")
        {
            GameObject statusBarPool = GameObject.FindGameObjectWithTag("Status Bar Pool");
            newObject.transform.SetParent(statusBarPool.transform);
        }
        
        return newObject;
    }
}
