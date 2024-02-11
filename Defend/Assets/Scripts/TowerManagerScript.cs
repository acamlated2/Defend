using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    
    public HashSet<GameObject> Towers = new HashSet<GameObject>();

    public void AddToSet(GameObject tower)
    {
        Towers.Add(tower);
    }

    public void RemoveTowerFromSet(GameObject tower)
    {
        if (Towers.Contains(tower))
        {
            Towers.Remove(tower);
        }
    }

    public void RemoveEnemyFromTower(GameObject enemy)
    {
        foreach (var tower in Towers)
        {
            tower.GetComponent<BaseTowerScript>().RemoveEnemyFromSet(enemy);
        }
    }

    public void CreateNewTower(Vector3 position)
    {
        GameObject newTower = Instantiate(towerPrefab, position, Quaternion.identity);
        AddToSet(newTower);
    }
}
