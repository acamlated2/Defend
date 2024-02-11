using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        return newObject;
    }
    
    public void ReturnObject(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
    }

    private GameObject InstantiateNewObject()
    {
        GameObject newObject = Instantiate(objectPrefab, transform);
        newObject.SetActive(false);
        _pool.Add(newObject);
        newObject.transform.parent = transform;
        return newObject;
    }
}
